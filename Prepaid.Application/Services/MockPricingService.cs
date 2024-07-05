namespace Prepaid.Application.Services;

public class MockPricingService : IMockPricingService
{
    public Task<decimal> CalculatePrice(DateTime startTime, DateTime endTime,
        CancellationToken cancellationToken = default)
    {
        {
            return Task.FromResult<decimal>(10);
        }
    }
}


public class MockPaymentService : IMockPaymentService
{
    public Task<string> Create(Guid uniqueId, decimal amount, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<string>("www.adyen.payment&bookingid=123132123");
    }

    public Task MakePartialRefund(Guid uniqueId, string paymentId, decimal amount, CancellationToken cancellationToken = default)
    {
       return Task.CompletedTask;
    }
}

public interface IMockPricingService
{
    Task<decimal> CalculatePrice(DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default);
}

public interface IMockPaymentService
{
    Task<string> Create(Guid uniqueId, decimal amount, CancellationToken cancellationToken = default);
    Task MakePartialRefund(Guid uniqueId, string paymentId, decimal amount, CancellationToken cancellationToken = default);
}