namespace Prepaid.Api.Responses;

public class UpdateBookingApiResponse
{
    public Guid UniqueId { get; set; }
    public string? PaymentUrl { get; set; }
}