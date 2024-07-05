using Prepaid.Domain.Exceptions;

namespace Prepaid.Domain.Models.States;

public class RefundedState : IBookingState
{
    private readonly Booking _booking;

    public RefundedState(Booking booking)
    {
        _booking = booking;
    }

    public void SetPaid()
    {
        throw new StateTransitionNotAllowedDomainException($"Booking state transition not allowed from {_booking.BookingSate} to {nameof(BookingState.Paid)}");
    }

    public void SetPending()
    {
        throw new StateTransitionNotAllowedDomainException($"Booking state transition not allowed from {_booking.BookingSate} to {nameof(BookingState.Pending)}");
    }

    public void SetRefunded()
    {
        throw new StateTransitionNotAllowedDomainException($"Already in same state {_booking.BookingSate}");
    }

    public void SetExpired()
    {
        _booking.SetExpiredState();
    }

    public BookingState BookingState => BookingState.RefundedState;
}