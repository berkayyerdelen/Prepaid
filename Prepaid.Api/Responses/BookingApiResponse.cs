using Prepaid.Domain.Models;

namespace Prepaid.Api.Responses;

public class BookingApiResponse
{
    public Guid UniqueId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public BookingState BookingState { get; set; }
    public Guid UserId { get; set; }
}