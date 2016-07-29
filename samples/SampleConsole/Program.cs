using System;
using WinBeacon;
using WinBeacon.Stack.Hci;

namespace SampleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var hub = new BeaconHub(0x0A5C, 0x21E8))
            {
                hub.BeaconDetected += (sender, e) => Console.WriteLine(e.Beacon.ToString());
                hub.StoreAdvertisingParameters(200, 200, AdvertisingType.ADV_IND, OwnAddressType.Public, PeerAddressType.Public, "000000000000", AdvertisingChannelMap.ChannelAll, AdvertisingFilterPolicy.ConnectAllScanAll);
                hub.EnableAdvertising(new Beacon("B9407F30-F5F8-466E-AFF9-25556B57FE6D", 1000, 2000, -52));
                
                Console.ReadKey();
            }
        }
    }
}
