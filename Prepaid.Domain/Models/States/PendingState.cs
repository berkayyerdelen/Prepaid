using Prepaid.Domain.Exceptions;

namespace Prepaid.Domain.Models.States;

public class PendingState: IBookingState
{
    private readonly Booking _booking;

    public PendingState(Booking booking)
    {
        _booking = booking;
    }

    public void SetPaid()
    {
        _booking.SetPaidState();
    }

    public void SetPending()
    {
        throw new StateTransitionNotAllowedDomainException($"Already in same state {_booking.BookingSate}");
    }

    public void SetRefunded()
    {
        throw new StateTransitionNotAllowedDomainException($"Booking state transition not allowed from {_booking.BookingSate} to {BookingState.RefundedState}");
    }

    public void SetExpired()
    {
        _booking.SetExpiredState();
    }

    public BookingState BookingState => BookingState.Pending;
}