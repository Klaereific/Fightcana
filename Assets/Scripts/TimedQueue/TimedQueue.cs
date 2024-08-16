using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class TimedQueue<T>
{
    private Queue<TimedQueueElement<T>> queue;
    private readonly TimeSpan expirationTime;  // Time after which elements expire
    private Timer timer;
    public int Count;

    public TimedQueue(int capacity, double expirationSeconds, double checkIntervalSeconds = 1.0)
    {
        queue = new Queue<TimedQueueElement<T>>(capacity);
        expirationTime = TimeSpan.FromSeconds(expirationSeconds);
        timer = new Timer(RemoveExpiredElements, null, TimeSpan.Zero, TimeSpan.FromSeconds(checkIntervalSeconds));
        Count = 0;
    }

    public void Enqueue(T element)
    {
        var timeAdded = DateTime.UtcNow;
        queue.Enqueue(new TimedQueueElement<T>(element, timeAdded));
        Count += 1;
        Debug.Log("Item enqueued");
    }

    public T Dequeue()
    {
        RemoveExpiredElements(null);  // Clean up expired elements before dequeuing
        if (queue.Count > 0)
        {
            Count -= 1;
            return queue.Dequeue().Value;
        }
        Debug.Log("Item dequeued");
        return default(T);
    }

    private void RemoveExpiredElements(object state)
    {
        DateTime currentTime = DateTime.UtcNow;
        while (queue.Count > 0 && currentTime - queue.Peek().TimeAdded > expirationTime)
        {
            queue.Dequeue();
            Debug.Log("Item removed");
            Count -= 1;
        }
    }

    public void Dispose()
    {
        timer.Dispose();
    }
    public TimedQueueElement<T> Peek()
    {
        TimedQueueElement<T> element = queue.Peek();
        return element;
    }
}
