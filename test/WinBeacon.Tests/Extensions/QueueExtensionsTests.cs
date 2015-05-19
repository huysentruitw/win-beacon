using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace WinBeacon.Tests.Extensions
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
    }
}
