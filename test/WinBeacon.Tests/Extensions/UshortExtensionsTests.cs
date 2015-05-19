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

using NUnit.Framework;

namespace WinBeacon.Tests.Extensions
{
    [TestFixture]
    public class UshortExtensionsTests
    {
        [Test]
        public void UshortExtensions_LoByte()
        {
            ushort input = 0x5386;
            byte output = input.LoByte();
            Assert.AreEqual(0x86, output);
        }

        [Test]
        public void UshortExtensions_HiByte()
        {
            ushort input = 0x8429;
            byte output = input.HiByte();
            Assert.AreEqual(0x84, output);
        }
    }
}
