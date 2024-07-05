namespace Prepaid.Api.Responses;

public class CreateBookingApiResponse
{
    public Guid UniqueId { get; set; }
    public string PaymentUrl { get; set; }
}