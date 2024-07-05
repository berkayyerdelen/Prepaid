using Prepaid.Domain.Exceptions;

namespace Prepaid.Domain.Models.States;

public class ExpiredState : IBookingState
{
    private readonly Booking _booking;

    public ExpiredState(Booking booking)
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
        throw new StateTransitionNotAllowedDomainException($"Booking state transition not allowed from {_booking.BookingSate} to {BookingState.Refunded}");
    }

    public void SetExpired()
    {
        throw new StateTransitionNotAllowedDomainException($"Booking state transition not allowed from {_booking.BookingSate} to {BookingState.Expired}");
    }

    public void SetCancelled()
    {
        throw new StateTransitionNotAllowedDomainException($"Booking state transition not allowed from {_booking.BookingSate} to {BookingState.Cancelled}");
    }

    public BookingState BookingState => BookingState.Expired;
}