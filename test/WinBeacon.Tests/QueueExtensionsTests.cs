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
using NUnit.Framework;
using WinBeacon.Stack;

namespace WinBeacon.Tests
{
    [TestFixture]
    public class QueueExtensionsTests
    {
        [Test]
        public void QueueExtensions_DequeueAll()
        {
            var input = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
            var queue = new Queue<byte>(input);
            byte[] output = queue.DequeueAll();
            Assert.AreEqual(input, output);
        }

        [Test]
        public void QueueExtensions_Dequeue_Exact()
        {
            var input = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 };
            var queue = new Queue<byte>(input);
            byte[] output = queue.Dequeue(3);
            Assert.AreEqual(new byte[] { 0x12, 0x34, 0x56 }, output);
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void QueueExtensions_Dequeue_TooMuch()
        {
            var input = new byte[] { 0x9A, 0xBC, 0xDE, 0xF0 };
            var queue = new Queue<byte>(input);
            queue.Dequeue(6);
        }

        [Test]
        public void QueueExtensions_Enqueue()
        {
            var input = new byte[] { 0x9A, 0xBC, 0xDE, 0xF0 };
            var queue = new Queue<byte>();
            queue.Enqueue(input);
            Assert.AreEqual(input.Length, queue.Count);
            Assert.AreEqual(input, queue.DequeueAll());
        }
    }
}
