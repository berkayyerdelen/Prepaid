using Prepaid.Domain.Models;
using Prepaid.Domain.Policies.Contracts;
using Prepaid.SharedKernel.Configurations;

namespace Prepaid.Domain.Policies;

public class BookingRefundPolicy : IBookingRefundPolicy
{
    private readonly BookingRefundPolicyConfiguration _bookingRefundPolicyConfiguration;

    public BookingRefundPolicy(BookingRefundPolicyConfiguration bookingRefundPolicyConfiguration)
    {
        _bookingRefundPolicyConfiguration = bookingRefundPolicyConfiguration;
    }

    public Task<bool> ApplyRefund(Booking booking, CancellationToken cancellationToken = default)
    {
        if (booking.BookingSate == BookingState.Paid)
        {
            return Task.FromResult(booking.AccessSlot.StartTime - DateTime.UtcNow >=
                                   TimeSpan.FromMinutes(_bookingRefundPolicyConfiguration.GracePeriodInMinute));    
        }

        return Task.FromResult(false);
    }
}