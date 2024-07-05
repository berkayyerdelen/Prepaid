using Prepaid.Application.Requests;
using Prepaid.Application.Responses;

namespace Prepaid.Application.Contracts;

public interface IBookingService
{
    Task<BookingApplicationResponse> Get(Guid uniqueId, CancellationToken cancellationToken = default);
    Task Create(BookingApplicationRequest request, CancellationToken cancellationToken = default);

    Task Refund(Guid uniqueId, CancellationToken cancellationToken = default);
}