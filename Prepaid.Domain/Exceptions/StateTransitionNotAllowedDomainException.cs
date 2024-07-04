using Prepaid.SharedKernel.Exceptions;

namespace Prepaid.Domain.Exceptions;

public class StateTransitionNotAllowedDomainException : DomainException
{
    public StateTransitionNotAllowedDomainException(string message) : base(message)
    {
    }
}