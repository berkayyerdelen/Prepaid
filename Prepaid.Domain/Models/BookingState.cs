namespace Prepaid.Domain.Models;

public enum BookingState
{
    Pending,
    Paid,
    Refunded,
    Expired,
    Cancelled
}