using Prepaid.Domain.Exceptions;

namespace Prepaid.Domain.Models;

public class AccessSlot
{
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    public AccessSlot SetAccessSlot(DateTime? startTime, DateTime endTime)
    {
        if (EndTime == default)
        {
            throw new InvalidAccessSlotParametersDomainException("Given argument doest not meet expectations");
        }

        StartTime = startTime.HasValue ? StartTime : DateTime.UtcNow;
        EndTime = endTime;

        return this;
    }
}