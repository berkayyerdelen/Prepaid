namespace Prepaid.Domain.Models.States;

public class CancelledState : IBookingState
{
    private readonly Booking _booking;

    public CancelledState(Booking booking)
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

    public void SetCancelled()
    {
        throw new StateTransitionNotAllowedDomainException($"Already in same state {_booking.BookingSate}");
    }

    public BookingState BookingState => BookingState.Cancelled;
}