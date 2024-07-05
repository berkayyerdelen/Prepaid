namespace Prepaid.Application.Requests;

public class CreateBookingApplicationRequest
{
    public Guid UserId { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime EndTime { get; set; }
}