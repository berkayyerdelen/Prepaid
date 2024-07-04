using Prepaid.Domain.Models.States;

namespace Prepaid.Domain.Models;

public class Booking
{
    public Booking(Guid uniqueId)
    {
        _bookingState = new PendingState(this);
        UniqueId = uniqueId;
    }

    public Booking()
    {
        _bookingState = new PendingState(this);
        UniqueId = Guid.NewGuid();
    }

    public Guid UniqueId { get; private set; }
    public AccessSlot AccessSlot { get; private set; }
    public Guid UserId { get; private set; }

    public PaymentInformation PaymentInformation { get; private set; }

    public void SetPaymentInformation(PaymentInformation paymentInformation)
    {
        PaymentInformation = paymentInformation;
    }

    public void SetUserId(Guid userId)
    {
        if (userId == default)
        {
            throw new InvalidUserIdParameterDomainException("User id can not be null!");
        }

        UserId = userId;
    }

    public void SetAccessSlot(AccessSlot accessSlot)
    {
        AccessSlot = accessSlot;
    }

    private IBookingState _bookingState;

    public BookingState BookingSate
    {
        get => _bookingState.BookingState;
        private set => _bookingState = StateResolver.Resolve(value, this);
    }

    public void ChangeState(IBookingState bookingState)
    {
        _bookingState = bookingState;
    }
}

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