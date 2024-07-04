using Prepaid.SharedKernel.Exceptions;

namespace Prepaid.Domain.Models;

public class StateTransitionNotAllowedDomainException : DomainException
{
    public StateTransitionNotAllowedDomainException(string message) : base(message)
    {
    }
}