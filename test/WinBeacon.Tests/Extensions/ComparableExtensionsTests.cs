/*
 * Copyright 2015-2019 Huysentruit Wouter
 *
 * See LICENSE file.
 */

using System;
using NUnit.Framework;

namespace WinBeacon.Tests.Extensions
{
    [TestFixture]
    public class ComparableExtensionsTests
    {
        [Test]
        public void Comparable_Equal()
        {
            Assert.IsNull("abc".NullableCompareTo("abc"), "Should return null when both items are equal");
        }

        [Test]
        public void Coparable_Bigger()
        {
            var result = (10).NullableCompareTo(2);
            Assert.NotNull(result);
            Assert.Greater(result.Value, 0, "Should return a value greater than zero");
        }

        [Test]
        public void Coparable_Smaller()
        {
            var result = (55.9).NullableCompareTo(56.0);
            Assert.NotNull(result);
            Assert.Less(result.Value, 0, "Should return a value greater than zero");
        }
    }
}
