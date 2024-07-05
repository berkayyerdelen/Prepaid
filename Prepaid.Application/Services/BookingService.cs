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

    public async Task Create(CreateBookingApplicationRequest request, CancellationToken cancellationToken = default)
    {
        var booking = new Booking();
        booking.SetAccessSlot(new AccessSlot().SetAccessSlot(request.StartTime, request.EndTime));
        booking.SetUserId(request.UserId);

        await _bookingRepository.Add(booking, cancellationToken);
    }

    public async Task Confirm(Guid uniqueId, PaymentInformationApplicationRequest request, string? partnerId = default,
        CancellationToken cancellationToken = default)
    {
        await _bookingRepository.Update(uniqueId, booking =>
        {
            booking.SetPaymentInformation(new PaymentInformation(request.PaymentId, request.PaymentToken,
                request.Amount, request.ServiceFee, request.PaymentDate));
            booking.SetPartnerId(partnerId!);
        }, cancellationToken);
    }

    public async Task<UpdateBookingApplicationResponse> Update(Guid uniqueId, UpdateBookingApplicationRequest request,
        CancellationToken cancellationToken = default)
    {
        //todo encapsulate login within domain layer gl!
        var initialBooking = await _bookingRepository.Get(uniqueId, cancellationToken);
        string paymentUrl = string.Empty;
        
        var booking = new Booking();
        booking.SetAccessSlot(new AccessSlot().SetAccessSlot(request.StartTime, request.EndTime));
        booking.SetUserId(request.UserId);

        var newPrice =
            await _mockPricingService.CalculatePrice(request.StartTime.Value, request.EndTime, cancellationToken);

        if (booking.PaymentInformation.Amount > newPrice)
        {
            var refundableAmount = initialBooking.PaymentInformation.Amount - newPrice;

            await _mockPaymentService.MakePartialRefund(initialBooking.UniqueId,
                initialBooking.PaymentInformation.PaymentId!,
                refundableAmount, cancellationToken);


            await _bookingRepository.Update(initialBooking.UniqueId, x => { x.SetCancelled(); }, cancellationToken);
            
            booking.SetPaymentInformation(new PaymentInformation(initialBooking.PaymentInformation.PaymentId, initialBooking.PaymentInformation.PaymentToken, newPrice, booking.PaymentInformation.ServiceFee,DateTime.UtcNow));
            booking.SetPaidState();
            booking.AddPriorPaymentInformation(new PriorPaymentInformation()
            {
                Amount = initialBooking.PaymentInformation.Amount,
                BookingId = initialBooking.UniqueId
            });
        }
        else if (booking.PaymentInformation.Amount < newPrice)
        {
            var extraAmount = newPrice - initialBooking.PaymentInformation.Amount;

             paymentUrl = await _mockPaymentService.Create(booking.UniqueId, extraAmount, cancellationToken);
             booking.SetPendingState();
        }
        
        await _bookingRepository.Add(booking, cancellationToken);
        
        return new UpdateBookingApplicationResponse()
        {
            UniqueId = booking.UniqueId,
            PaymentUrl = paymentUrl
        };
    }

    public async Task Refund(Guid uniqueId, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.Get(uniqueId, cancellationToken);

        await booking.ApplyRefund(_bookingRefundPolicy, cancellationToken);
    }
}