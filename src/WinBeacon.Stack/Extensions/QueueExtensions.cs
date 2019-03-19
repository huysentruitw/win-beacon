/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;
using System.Collections.Generic;

namespace WinBeacon.Stack
{
    /// <summary>
    /// Queue extension methods.
    /// </summary>
    public static class QueueExtensions
    {
        /// <summary>
        /// Dequeue all items from the queue.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="queue">The queue.</param>
        /// <returns>Array of dequeued items.</returns>
        public static T[] DequeueAll<T>(this Queue<T> queue)
        {
            var result = new List<T>();
            while (queue.Count > 0)
                result.Add(queue.Dequeue());
            return result.ToArray();
        }

        /// <summary>
        /// Dequeue the specified number of items.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="queue">The queue.</param>
        /// <param name="count">Number of items to dequeue.</param>
        /// <returns>Array of dequeued items.</returns>
        public static T[] Dequeue<T>(this Queue<T> queue, int count)
        {
            var result = new List<T>();
            for (int i = 0; i < count; i++)
                result.Add(queue.Dequeue());
            return result.ToArray();
        }

        /// <summary>
        /// Enqueue an array of items.
        /// </summary>
        /// <typeparam name="T">Item type.</typeparam>
        /// <param name="queue">The queue.</param>
        /// <param name="items">The items.</param>
        public static void Enqueue<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            foreach (T item in items)
                queue.Enqueue(item);
        }
    }
}
