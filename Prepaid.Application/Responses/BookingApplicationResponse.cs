using Prepaid.Domain.Models;

namespace Prepaid.Application.Responses;

public class BookingApplicationResponse
{
    public Guid UniqueId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public BookingState BookingState { get; set; }
    public Guid UserId { get; set; }
}