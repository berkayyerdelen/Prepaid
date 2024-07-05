namespace Prepaid.Application.Responses;

public class CreateBookingApplicationResponse
{
    public Guid UniqueId { get; set; }
    public string? PaymentUrl { get; set; }
}