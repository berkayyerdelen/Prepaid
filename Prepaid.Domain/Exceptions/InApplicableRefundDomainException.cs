using Prepaid.SharedKernel.Exceptions;

namespace Prepaid.Domain.Exceptions;

public class InApplicableRefundDomainException : DomainException
{
    public InApplicableRefundDomainException(string message) : base(message)
    {
    }
}