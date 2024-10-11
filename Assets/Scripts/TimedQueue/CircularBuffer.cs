using System;

public class CircularBuffer<T>
{
    private T[] buffer;
    private int head;     // Points to the index of the next write position
    private int tail;     // Points to the index of the next read position
    private int size;     // Current number of elements in the buffer
    private int capacity; // Maximum size of the buffer

    public CircularBuffer(int capacity)
    {
        this.capacity = capacity;
        buffer = new T[capacity];
        head = 0;
        tail = 0;
        size = 0;
    }

    // Add an element to the buffer
    public void Enqueue(T item)
    {
        buffer[head] = item;
        head = (head + 1) % capacity;  // Wrap around the buffer if the end is reached

        if (size < capacity)
        {
            size++;
        }
        else
        {
            // Buffer is full, so we advance the tail to maintain the circular buffer behavior
            tail = (tail + 1) % capacity;
        }
    }

    // Remove and return the oldest element from the buffer (FIFO)
    public T Dequeue()
    {
        if (size == 0)
        {
            throw new InvalidOperationException("Buffer is empty.");
        }

        T item = buffer[tail];
        tail = (tail + 1) % capacity;
        size--;
        return item;
    }

    // Peek at the oldest element without removing it
    public T Peek()
    {
        if (size == 0)
        {
            throw new InvalidOperationException("Buffer is empty.");
        }
        return buffer[tail];
    }

    // Get the number of elements currently stored in the buffer
    public int Count => size;

    // Get the maximum capacity of the buffer
    public int Capacity => capacity;

    // Print the buffer contents (for debugging purposes)
    public void PrintBuffer()
    {
        Console.WriteLine("Buffer contents:");
        for (int i = 0; i < size; i++)
        {
            int index = (tail + i) % capacity;
            Console.WriteLine(buffer[index]);
        }
    }
    
    public T[] ReturnBufferArray()
    {
        T[] output = new T[this.capacity];
        for (int i = 0; i < size; i++)
        {
            int index = (tail + i) % capacity;
            output[i] = buffer[index];
        }

        return output;
    }
    public T GetCurrentFrame()
    {
        return buffer[tail];
    }
}
