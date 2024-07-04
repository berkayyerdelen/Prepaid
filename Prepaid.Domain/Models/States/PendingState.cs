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
        _booking.ChangeState(new PaidState(_booking));
    }

    public void SetPending()
    {
        throw new StateTransitionNotAllowedDomainException($"Already in same state {_booking.BookingSate}");
    }

    public void SetCancelled()
    {
        _booking.ChangeState(new CancelledState(_booking));
    }

    public BookingState BookingState => BookingState.Pending;
}