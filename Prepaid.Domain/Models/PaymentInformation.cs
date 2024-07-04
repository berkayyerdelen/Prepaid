namespace Prepaid.Domain.Models;

public class PaymentInformation
{
    public PaymentInformation(string? paymentId, string? paymentToken, decimal amount, decimal serviceFee, DateTime? paymentTime)
    {
        PaymentId = paymentId;
        PaymentToken = paymentToken;
        Amount = amount;
        ServiceFee = serviceFee;
        PaymentTime = paymentTime;
    }

    public string? PaymentId { get; private set; }
    public string? PaymentToken { get; private set; }
    public decimal Amount { get; private set; } = 0.0m;
    public decimal ServiceFee { get; private set; } = 0.0m;
    public DateTime? PaymentTime { get; private set; }
}