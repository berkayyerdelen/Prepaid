using Prepaid.Domain.Exceptions;
using Prepaid.Domain.Models.States;
using Prepaid.Domain.Policies.Contracts;
namespace Prepaid.Domain.Models;

public class Booking
{
    public Booking(Guid uniqueId)
    {
        _bookingState = new PendingState(this);
        UniqueId = uniqueId;
    }

    public Booking()
    {
        _bookingState = new PendingState(this);
        UniqueId = Guid.NewGuid();
    }

    public Guid UniqueId { get; private set; }
    public AccessSlot AccessSlot { get; private set; }
    public Guid UserId { get; private set; }

    public PaymentInformation PaymentInformation { get; private set; }

    public void SetPaymentInformation(PaymentInformation paymentInformation)
    {
        PaymentInformation = paymentInformation;
    }

    public void SetUserId(Guid userId)
    {
        if (userId == default)
        {
            throw new InvalidUserIdParameterDomainException("User id can not be null!");
        }

        UserId = userId;
    }

    public void SetAccessSlot(AccessSlot accessSlot)
    {
        AccessSlot = accessSlot;
    }

    private IBookingState _bookingState;

    public BookingState BookingSate
    {
        get => _bookingState.BookingState;
        private set => _bookingState = StateResolver.Resolve(value, this);
    }

    public void SetPaidState() => _bookingState.SetPaid();
    public void SetRefundedState() => _bookingState.SetRefunded();
    public void SetPendingState() => _bookingState.SetPending();
    public void SetExpiredState() => _bookingState.SetExpired();

    public async Task ApplyRefund(IBookingRefundPolicy bookingRefundPolicy,
        CancellationToken cancellationToken = default)
    {
        var isRefundable = await bookingRefundPolicy.ApplyRefund(this, cancellationToken);

        if (!isRefundable)
        {
            throw new InApplicableRefundDomainException($"Ineligible booking: {UniqueId} for applying refund");
        }
    }
}