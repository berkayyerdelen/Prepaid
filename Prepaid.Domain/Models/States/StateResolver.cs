namespace Prepaid.Domain.Models.States;

public class StateResolver
{
    public static IBookingState Resolve(BookingState bookingState, Booking booking)
    {
        switch (bookingState)
        {
            case BookingState.Expired:
                return new ExpiredState(booking);
            case BookingState.Paid :
                return new PaidState(booking);
            case BookingState.Pending:
                return new PendingState(booking);
            case BookingState.Refunded:
                return new RefundedState(booking);
            case BookingState.Cancelled:
                return new CancelledState(booking);
            default:
                throw new ArgumentNullException(nameof(bookingState));
        }
    }
}