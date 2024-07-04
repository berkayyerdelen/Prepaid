using Prepaid.Domain.Exceptions;
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

    internal void ChangeState(IBookingState bookingState)
    {
        _bookingState = bookingState;
    }

    public void SetPaidState() => _bookingState.SetPaid();
    public void SetCancelledState() => _bookingState.SetCancelled();
    public void SetPendingState() => _bookingState.SetPending();
}