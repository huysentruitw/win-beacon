using System;
using WinBeacon;

namespace SampleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var hub = new BeaconHub(0x050D, 0x065A))
            {
                hub.BeaconDetected += (sender, e) => Console.WriteLine(e.Beacon.ToString());

                hub.EnableAdvertising(new Beacon("", 1, 135, -52));

                Console.ReadKey();
            }
        }
    }
}
