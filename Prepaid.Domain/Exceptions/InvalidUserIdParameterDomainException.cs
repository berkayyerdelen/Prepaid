using Prepaid.SharedKernel.Exceptions;

namespace Prepaid.Domain.Models;

public class InvalidUserIdParameterDomainException : DomainException
{
    public InvalidUserIdParameterDomainException(string message) : base(message)
    {
    }
}