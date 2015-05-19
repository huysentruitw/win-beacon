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
