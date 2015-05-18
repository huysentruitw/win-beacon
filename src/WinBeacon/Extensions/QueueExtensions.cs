/*
 * Copyright 2015 Huysentruit Wouter
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Collections.Generic;

namespace WinBeacon
{
    public static class QueueExtensions
    {
        public static T[] DequeueAll<T>(this Queue<T> queue)
        {
            var result = new List<T>();
            while (queue.Count > 0)
                result.Add(queue.Dequeue());
            return result.ToArray();
        }

        public static T[] Dequeue<T>(this Queue<T> queue, int count)
        {
            var result = new List<T>();
            for (int i = 0; i < Math.Min(count, queue.Count); i++)
                result.Add(queue.Dequeue());
            return result.ToArray();
        }
    }
}
