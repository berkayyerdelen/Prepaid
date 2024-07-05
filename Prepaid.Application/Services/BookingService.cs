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

    public BookingService(IBookingRepository bookingRepository,
        IBookingRefundPolicy bookingRefundPolicy)
    {
        _bookingRepository = bookingRepository;
        _bookingRefundPolicy = bookingRefundPolicy;
    }

    public async Task<BookingApplicationResponse> Get(Guid uniqueId, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.Get(uniqueId, cancellationToken);

        return booking.ToApplicationResponse();
    }

    public async Task Create(BookingApplicationRequest request, CancellationToken cancellationToken = default)
    {
        var booking = new Booking();
        booking.SetAccessSlot(new AccessSlot().SetAccessSlot(request.StartTime, request.EndTime));
        booking.SetUserId(request.UserId);

        // call pricing service
        // call payment service to fill payment information in booking entity

        await _bookingRepository.Add(booking, cancellationToken);
    }

    public async Task Confirm(Guid uniqueId, PaymentInformationApplicationRequest request, string? partnerId = default, CancellationToken cancellationToken = default)
    {
        await _bookingRepository.Update(uniqueId, booking =>
        {
            booking.SetPaymentInformation(new PaymentInformation(request.PaymentId, request.PaymentToken,
                request.Amount, request.ServiceFee, request.PaymentDate));
            booking.SetPartnerId(partnerId!);
        }, cancellationToken);
    }

    public async Task Refund(Guid uniqueId, CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.Get(uniqueId, cancellationToken);

        await booking.ApplyRefund(_bookingRefundPolicy, cancellationToken);
    }
}