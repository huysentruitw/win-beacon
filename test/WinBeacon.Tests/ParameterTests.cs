/*
 * Copyright 2015-2016 Huysentruit Wouter
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
using WinBeacon.Stack.Hci.Parameters;

namespace WinBeacon.Tests
{
    [TestFixture]
    public class ParameterTests
    {
        [Test]
        public void Parameter_BoolCommandParameter()
        {
            var parameter = new BoolCommandParameter(false);
            Assert.AreEqual(new byte[] { 0x00 }, parameter.ToByteArray());
            parameter = new BoolCommandParameter(true);
            Assert.AreEqual(new byte[] { 0x01 }, parameter.ToByteArray());
        }

        [Test]
        public void Parameter_ByteArrayCommandParameter()
        {
            var data = new byte[] { 0x01, 0xAA, 0x55 };
            var parameter = new ByteArrayCommandParameter(data);
            Assert.AreEqual(data, parameter.ToByteArray());
        }

        [Test]
        public void Parameter_ByteCommandParameter()
        {
            var parameter = new ByteCommandParameter(0x57);
            Assert.AreEqual(new byte[] { 0x57 }, parameter.ToByteArray());
        }

        [Test]
        public void Parameter_UshortCommandParameter()
        {
            var parameter = new UshortCommandParameter(0x5865);
            Assert.AreEqual(new byte[] { 0x65, 0x58 }, parameter.ToByteArray());
        }
    }
}
