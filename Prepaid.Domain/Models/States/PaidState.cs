using Prepaid.Domain.Exceptions;

namespace Prepaid.Domain.Models.States;

public class PaidState : IBookingState
{
    private readonly Booking _booking;

    public PaidState(Booking booking)
    {
        _booking = booking;
    }

    public void SetPaid()
    {
        throw new StateTransitionNotAllowedDomainException($"Already in same state {_booking.BookingSate}");
    }

    public void SetPending()
    {
        throw new StateTransitionNotAllowedDomainException($"Booking state transition not allowed from {_booking.BookingSate} to {BookingState.Pending}");
    }


    public void SetRefunded()
    {
        _booking.SetRefundedState();
    }

    public void SetExpired()
    {
        throw new StateTransitionNotAllowedDomainException($"Booking state transition not allowed from {_booking.BookingSate} to {BookingState.Expired}");
    }

    public void SetCancelled()
    {
        _booking.SetCancelled();
    }

    public BookingState BookingState => BookingState.Paid;
}