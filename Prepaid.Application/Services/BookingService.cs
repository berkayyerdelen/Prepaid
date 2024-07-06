using Prepaid.Application.Contracts;
using Prepaid.Application.Extensions.MappingExtensions;
using Prepaid.Application.Requests;
using Prepaid.Application.Responses;
using Prepaid.Domain.Models;
using Prepaid.Domain.Policies.Contracts;
using Prepaid.Domain.Repositories;

namespace Prepaid.Application.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IBookingRefundPolicy _bookingRefundPolicy;
    private readonly IMockPricingService _mockPricingService;
    private readonly IMockPaymentService _mockPaymentService;

    public BookingService(IBookingRepository bookingRepository,
        IBookingRefundPolicy bookingRefundPolicy,
        IMockPricingService mockPricingService, IMockPaymentService mockPaymentService)
    {
        _bookingRepository = bookingRepository;
        _bookingRefundPolicy = bookingRefundPolicy;
        _mockPricingService = mockPricingService;
        _mockPaymentService = mockPaymentService;
    }

    public async Task<BookingApplicationResponse> Get(Guid uniqueId, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.Get(uniqueId, cancellationToken);

        return booking.ToApplicationResponse();
    }

    public async Task<CreateBookingApplicationResponse> Create(CreateBookingApplicationRequest request,
        CancellationToken cancellationToken = default)
    {
        var booking = new Booking();
        booking.SetAccessSlot(new AccessSlot().SetAccessSlot(request.StartTime, request.EndTime));
        booking.SetUserId(request.UserId);

        var price = await _mockPricingService.CalculatePrice(request.StartTime.Value, request.EndTime,
            cancellationToken);
        var paymentUrl = await _mockPaymentService.Create(booking.UniqueId, price, cancellationToken);

        await _bookingRepository.Add(booking, cancellationToken);

        return new CreateBookingApplicationResponse()
        {
            UniqueId = booking.UniqueId,
            PaymentUrl = paymentUrl
        };
    }

    public async Task Confirm(Guid uniqueId, PaymentInformationApplicationRequest request, string? partnerId = default,
        CancellationToken cancellationToken = default)
    {
        await _bookingRepository.Update(uniqueId, booking =>
        {
            booking.SetPaymentInformation(new PaymentInformation(request.PaymentId, request.PaymentToken,
                request.Amount, request.ServiceFee, request.PaymentDate));
            booking.SetPartnerId(partnerId!);
            booking.SetPaidState();
        }, cancellationToken);
    }

    public async Task<UpdateBookingApplicationResponse> Update(Guid uniqueId, UpdateBookingApplicationRequest request,
        CancellationToken cancellationToken = default)
    {
        var initialBooking = await _bookingRepository.Get(uniqueId, cancellationToken);
        string paymentUrl = string.Empty;
        var booking = new Booking();
        var isRefundable = await initialBooking.CheckRefundable(_bookingRefundPolicy, cancellationToken);

        if (isRefundable)
        {
            booking.SetAccessSlot(new AccessSlot().SetAccessSlot(request.StartTime, request.EndTime));
            booking.SetUserId(request.UserId);

            var newAmount =
                await _mockPricingService.CalculatePrice(request.StartTime.Value, request.EndTime, cancellationToken);

            var refundableAmount =
                await initialBooking.CalculateRefundableAmount(_bookingRefundPolicy, newAmount, cancellationToken);

            if (refundableAmount >= 0.0m)
            {
                await _mockPaymentService.MakePartialRefund(initialBooking.UniqueId,
                    initialBooking.PaymentInformation.PaymentId!, refundableAmount, cancellationToken);

                await _bookingRepository.Update(initialBooking.UniqueId, x => { x.SetCancelled(); }, cancellationToken);

                var newBookingAmount = initialBooking.CalculateActualAmountAfterRefund(refundableAmount);
                booking.SetPaymentInformation(new PaymentInformation(initialBooking.PaymentInformation.PaymentId,
                    initialBooking.PaymentInformation.PaymentToken, newBookingAmount,
                    initialBooking.PaymentInformation.ServiceFee, DateTime.UtcNow));
                booking.AddPriorPaymentInformation(new PriorPaymentInformation()
                {
                    Amount = initialBooking.PaymentInformation.Amount,
                    BookingId = initialBooking.UniqueId
                });
                booking.SetPaidState();
            }
            else
            {
                var extraAmount = initialBooking.CalculateAdditionalCostForBookingUpdate(newAmount);

                paymentUrl = await _mockPaymentService.Create(booking.UniqueId, extraAmount, cancellationToken);
                booking.SetPendingState();
            }

            await _bookingRepository.Add(booking, cancellationToken);
        }

        return new UpdateBookingApplicationResponse()
        {
            UniqueId = booking.UniqueId,
            PaymentUrl = paymentUrl
        };
    }

    public async Task Refund(Guid uniqueId, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.Get(uniqueId, cancellationToken);

        var isRefundable = await booking.CheckRefundable(_bookingRefundPolicy, cancellationToken);

        if (isRefundable)
        {
            await _mockPaymentService.MakePartialRefund(booking.UniqueId, booking.PaymentInformation.PaymentId,
                booking.PaymentInformation.Amount,
                cancellationToken);
        }
    }
}