using Prepaid.SharedKernel.Exceptions;

namespace Prepaid.Domain.Exceptions;

public class InvalidAccessSlotParametersDomainException: DomainException
{
    public InvalidAccessSlotParametersDomainException(string message) : base(message)
    {
    }
}