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
using Moq;
using NUnit.Framework;

namespace WinBeacon.Tests
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
