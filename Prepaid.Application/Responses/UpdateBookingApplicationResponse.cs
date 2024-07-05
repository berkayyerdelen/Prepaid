namespace Prepaid.Application.Responses;

public class UpdateBookingApplicationResponse
{
    public Guid UniqueId { get; set; }
    public string? PaymentUrl { get; set; }
}