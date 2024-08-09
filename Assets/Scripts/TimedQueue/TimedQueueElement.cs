using System;
public class TimedQueueElement<T>
{
    public T Value { get; }
    public DateTime TimeAdded { get; }

    public TimedQueueElement(T value, DateTime timeAdded)
    {
        Value = value;
        TimeAdded = DateTime.UtcNow;
    }
}

