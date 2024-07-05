using Prepaid.Domain.Models;

namespace Prepaid.Domain.Repositories;

public interface IBookingRepository
{
    Task<Booking> Get(Guid uniqueId, CancellationToken cancellationToken = default);
    Task Add(Booking booking, CancellationToken cancellationToken = default);
    Task Update(Guid uniqueId, Action<Booking> act, CancellationToken cancellationToken);
}