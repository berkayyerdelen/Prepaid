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
        throw new StateTransitionNotAllowedDomainException($"Booking state transition not allowed from {_booking.BookingSate} to {BookingState.Paid}");
    }

    public void SetPending()
    {
        throw new StateTransitionNotAllowedDomainException($"Booking state transition not allowed from {_booking.BookingSate} to {BookingState.Pending}");
    }

    public void SetRefunded()
    {
        throw new StateTransitionNotAllowedDomainException($"Already in same state {_booking.BookingSate}");
    }

    public void SetExpired()
    {
        _booking.SetExpiredState();
    }

    public void SetCancelled()
    {
        throw new StateTransitionNotAllowedDomainException($"Booking state transition not allowed from {_booking.BookingSate} to {BookingState.Cancelled}");
    }

    public BookingState BookingState => BookingState.Refunded;
}