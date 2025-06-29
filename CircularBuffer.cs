using System;
using System.ComponentModel;
using System.Threading;

namespace Adrien007.Utilities.Arrays;

#region Buffer circulaire
    /// <summary>
    /// A simple and generic implementation of a circular buffer.
    /// </summary>
    /// <typeparam name="T">Everything!</typeparam>
    public class CircularBuffer<T>
    {
        private T[] _buffer;
        private int _head;
        private int _tail;
        private int _size;
        private int _capacity;

        public CircularBuffer(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentException("Capacity must be greater than zero.");

            _capacity = capacity;
            _buffer = new T[capacity];
            _head = 0;
            _tail = 0;
            _size = 0;
        }

        public void Enqueue(T item)
        {
            if (!CanEnqueue())
                throw new InvalidOperationException("Buffer is full."); // Prevent overwriting data

            _buffer[_tail] = item;
            _tail = (_tail + 1) % _capacity; // Wrap around to the start if the end is reached
            _size++;
        }

        public T Dequeue()
        {
            if (!CanDequeue())
                throw new InvalidOperationException("Buffer is empty.");

            T item = _buffer[_head];
            _head = (_head + 1) % _capacity;
            _size--;
            return item;
        }

        public T Peek()
        {
            if (!CanDequeue())
                throw new InvalidOperationException("Buffer is empty.");

            return _buffer[_head];
        }


        public bool CanEnqueue() { return _size != _capacity; }
        public bool CanDequeue() { return _size > 0; }

        public int Count => _size;

        public int Capacity => _capacity;
    }
    /// <summary>
    /// A thread-safe wrapper around the CircularBuffer class.
    /// </summary>
    /// <typeparam name="T">Everything!</typeparam>
    public class CircularBufferThreadSafe<T>
    {
        /// <inheritdoc />
        private CircularBuffer<T> circularBuffer;
        private readonly object _lock = new object();

        public CircularBufferThreadSafe(int capacity)
        {
            circularBuffer = new CircularBuffer<T>(capacity);
        }


        public void Enqueue(T item)
        {
            lock (_lock)
            {
                circularBuffer.Enqueue(item);
            }
        }

        public T Dequeue()
        {
            lock (_lock)
            {
                return circularBuffer.Dequeue();
            }
        }

        public T Peek()
        {
            lock (_lock)
            {
                return circularBuffer.Peek();
            }
        }

        public bool CanEnqueue() { return circularBuffer.CanEnqueue(); }

        public bool CanDequeue() { return circularBuffer.CanDequeue(); }

        public int Count => circularBuffer.Count;

        public int Capacity => circularBuffer.Capacity;
    }
#endregion
