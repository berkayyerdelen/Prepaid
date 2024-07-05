using Prepaid.Domain.Models;

namespace Prepaid.Domain.Policies.Contracts;

public interface IBookingRefundPolicy
{
    Task<bool> CheckRefundable(Booking booking, CancellationToken cancellationToken = default);
    Task<decimal> CalculateRefundableAmount(Booking booking, decimal newAmount, CancellationToken cancellationToken);
}