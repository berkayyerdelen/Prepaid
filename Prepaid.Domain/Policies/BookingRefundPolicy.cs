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

    public Task<bool> CheckRefundable(Booking booking, CancellationToken cancellationToken = default)
    {
        if (booking.BookingSate == BookingState.Paid)
        {
            return Task.FromResult(booking.AccessSlot.StartTime - DateTime.UtcNow >=
                                   TimeSpan.FromMinutes(_bookingRefundPolicyConfiguration.GracePeriodInMinute));    
        }

        return Task.FromResult(false);
    }

    public Task<decimal> CalculateRefundableAmount(Booking booking, decimal newAmount, CancellationToken cancellationToken)
    {
        if (booking.PaymentInformation.Amount > newAmount)
        {
            var refundableAmount = booking.PaymentInformation.Amount - newAmount;

            return Task.FromResult(refundableAmount);
        }

        return Task.FromResult(0.0m);
    }
}