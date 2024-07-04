using Prepaid.SharedKernel.Exceptions;

namespace Prepaid.Domain.Exceptions;

public class InvalidUserIdParameterDomainException : DomainException
{
    public InvalidUserIdParameterDomainException(string message) : base(message)
    {
    }
}