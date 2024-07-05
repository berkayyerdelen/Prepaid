using Prepaid.Application.Responses;
using Prepaid.Domain.Models;

namespace Prepaid.Application.Extensions.MappingExtensions;

public static class BookingMappingExtensions
{
    public static BookingApplicationResponse ToApplicationResponse(this Booking booking)
    {
        return new BookingApplicationResponse()
        {
            UniqueId = booking.UniqueId,
            BookingState = booking.BookingSate,
            EndTime = booking.AccessSlot.EndTime,
            StartTime = booking.AccessSlot.StartTime,
            UserId = booking.UserId
        };
    }
}