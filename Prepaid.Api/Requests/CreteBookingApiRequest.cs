namespace Prepaid.Api.Requests;

public class CreteBookingApiRequest
{
    public Guid UserId { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime EndTime { get; set; }
}