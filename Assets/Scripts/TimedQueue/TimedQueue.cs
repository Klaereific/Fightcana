using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class TimedQueue<T>
{
    private Queue<TimedQueueElement<T>> queue;
    private readonly TimeSpan expirationTime;  // Time after which elements expire
    private Timer timer;

    public TimedQueue(int capacity, double expirationSeconds, double checkIntervalSeconds = 1.0)
    {
        queue = new Queue<TimedQueueElement<T>>(capacity);
        expirationTime = TimeSpan.FromSeconds(expirationSeconds);
        timer = new Timer(RemoveExpiredElements, null, TimeSpan.Zero, TimeSpan.FromSeconds(checkIntervalSeconds));
    }

    public void Enqueue(T element)
    {
        var timeAdded = DateTime.UtcNow;
        queue.Enqueue(new TimedQueueElement<T>(element, timeAdded));
    }

    public T Dequeue()
    {
        RemoveExpiredElements(null);  // Clean up expired elements before dequeuing
        if (queue.Count > 0)
        {
            return queue.Dequeue().Value;
        }
        return default(T);
    }

    private void RemoveExpiredElements(object state)
    {
        DateTime currentTime = DateTime.UtcNow;
        while (queue.Count > 0 && currentTime - queue.Peek().TimeAdded > expirationTime)
        {
            queue.Dequeue();
        }
    }

    public void Dispose()
    {
        timer.Dispose();
    }
}
