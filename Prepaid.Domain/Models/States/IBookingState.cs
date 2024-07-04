namespace Prepaid.Domain.Models.States;

public interface IBookingState
{
    void SetPaid();
    void SetPending();
    void SetCancelled();
    BookingState BookingState { get; }
}