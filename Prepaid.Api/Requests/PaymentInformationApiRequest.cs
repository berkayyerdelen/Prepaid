namespace Prepaid.Api.Requests;

public class PaymentInformationApiRequest
{
    public string? PaymentId { get; set; }
    public string? PaymentToken { get; set; }
    public decimal Amount { get; set; }
    public decimal ServiceFee { get; set; }
    public DateTime PaymentDate { get; set; }
}