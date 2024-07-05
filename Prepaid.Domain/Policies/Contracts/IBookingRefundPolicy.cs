using Prepaid.Domain.Models;

namespace Prepaid.Domain.Policies.Contracts;

public interface IBookingRefundPolicy
{
    Task<bool> ApplyRefund(Booking booking, CancellationToken cancellationToken = default);
}