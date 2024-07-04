using MongoDB.Driver;
using Prepaid.Domain.Models;
using Prepaid.Domain.Repositories;
using Prepaid.Infrastructure.Persistence;

namespace Prepaid.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationContext _applicationContext;

    public BookingRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public Task<Booking> Get(Guid uniqueId, CancellationToken cancellationToken = default)
    {
        var filterDefinition = new FilterDefinitionBuilder<Booking>().Eq(x => x.UniqueId, uniqueId);
        return _applicationContext.Bookings.Find(filterDefinition).FirstOrDefaultAsync(cancellationToken);
    }

    public Task Add(Booking booking, CancellationToken cancellationToken = default)
    {
        return _applicationContext.Bookings.InsertOneAsync(booking,new InsertOneOptions(), cancellationToken);
    }
}