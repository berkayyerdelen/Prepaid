namespace Prepaid.Application.Requests;

public class UpdateBookingApplicationRequest
{
    public Guid UniqueId { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Guid UserId { get; set; }
}