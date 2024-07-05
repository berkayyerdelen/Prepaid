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

    public string? PartnerId { get; private set; }

    public PaymentInformation PaymentInformation { get; private set; }

    public IReadOnlyCollection<PriorPaymentInformation> PriorPaymentInformation =>
        _priorPaymentInformation.AsReadOnly();

    public IList<PriorPaymentInformation> _priorPaymentInformation;
    public void AddPriorPaymentInformation(PriorPaymentInformation priorPaymentInformation)
    {
        _priorPaymentInformation.Add(priorPaymentInformation);
    }
    
    public void SetPaymentInformation(PaymentInformation paymentInformation)
    {
        PaymentInformation = paymentInformation;
    }

    public void SetPartnerId(string partnerId)
    {
        PartnerId = partnerId;
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
    public void SetCancelled() => _bookingState.SetCancelled();

    public async Task<bool> CheckRefundable(IBookingRefundPolicy bookingRefundPolicy,
        CancellationToken cancellationToken = default)
    {
       return await bookingRefundPolicy.CheckRefundable(this, cancellationToken);
    }

    public async Task<decimal> CalculateRefundableAmount(IBookingRefundPolicy bookingRefundPolicy, decimal newAmount, CancellationToken cancellationToken)
    {
        return await bookingRefundPolicy.CalculateRefundableAmount(this, newAmount, cancellationToken);
    }

    public decimal CalculateActualAmountAfterRefund(decimal refundableAmount)
    {
        return PaymentInformation.Amount - refundableAmount;
    }

    public decimal CalculateAdditionalCostForBookingUpdate(decimal newAmount)
    {
        return newAmount - PaymentInformation.Amount;
    }
}

public class PriorPaymentInformation
{
    public Guid BookingId { get; set; }
    public decimal Amount { get; set; }
}