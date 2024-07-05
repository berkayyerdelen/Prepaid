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
        throw new NotImplementedException();
    }

    public void SetPending()
    {
        throw new NotImplementedException();
    }

    public void SetRefunded()
    {
        throw new NotImplementedException();
    }

    public void SetExpired()
    {
        throw new NotImplementedException();
    }

    public BookingState BookingState => BookingState.Expired;
}