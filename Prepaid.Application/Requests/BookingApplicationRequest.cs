namespace Prepaid.Application.Requests;

public class BookingApplicationRequest
{
    public Guid UserId { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime EndTime { get; set; }
}