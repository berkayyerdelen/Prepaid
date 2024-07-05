using Prepaid.Application.Requests;
using Prepaid.Application.Responses;

namespace Prepaid.Application.Contracts;

public interface IBookingService
{
    Task<BookingApplicationResponse> Get(Guid uniqueId, CancellationToken cancellationToken = default);
    Task Create(CreateBookingApplicationRequest request, CancellationToken cancellationToken = default);

    Task Refund(Guid uniqueId, CancellationToken cancellationToken = default);

    Task Confirm(Guid uniqueId, PaymentInformationApplicationRequest request, string? partnerId = default, CancellationToken cancellationToken = default);

    Task<UpdateBookingApplicationResponse> Update(Guid uniqueId, UpdateBookingApplicationRequest request, CancellationToken cancellationToken = default);
}