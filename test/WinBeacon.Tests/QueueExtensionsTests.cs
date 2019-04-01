/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
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
        public void QueueExtensions_Dequeue_TooMuch()
        {
            var input = new byte[] { 0x9A, 0xBC, 0xDE, 0xF0 };
            var queue = new Queue<byte>(input);

            Assert.Throws<InvalidOperationException>(() => queue.Dequeue(6));
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
