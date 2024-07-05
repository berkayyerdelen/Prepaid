namespace Prepaid.Api.Requests;

public class UpdateBookingApiRequest
{
    public DateTime? StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Guid UserId { get; set; }
}