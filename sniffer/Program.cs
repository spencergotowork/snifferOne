using System;
using PacketDotNet;
using SharpPcap;

namespace sniffer
{
    class Program
    {
        private static void Device_OnPacketArrival(object s, PacketCapture e)
        {
            var time = e.Header.Timeval.Date;
            var len = e.Data.Length;
            var rawPacket = e.GetPacket();

            //Console.WriteLine("+++++++++++ LinkLayerType : {0} ", rawPacket.LinkLayerType.ToString());
            var packet = PacketDotNet.Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);
            
            var tcpPacket = packet.Extract<PacketDotNet.TcpPacket>();
            if (tcpPacket != null)
            {
                var ipPacket = (PacketDotNet.IPPacket)tcpPacket.ParentPacket;
                System.Net.IPAddress srcIp = ipPacket.SourceAddress;
                System.Net.IPAddress dstIp = ipPacket.DestinationAddress;
                int srcPort = tcpPacket.SourcePort;
                int dstPort = tcpPacket.DestinationPort;

                Console.WriteLine("{0}:{1}:{2},{3} Len={4} {5}:{6} -> {7}:{8}",
                    time.Hour, time.Minute, time.Second, time.Millisecond, len,
                    srcIp, srcPort, dstIp, dstPort);
            }
        }
        static void Main(string[] args)
        {
            var devices = CaptureDeviceList.Instance;
            int idx = 0;
            foreach (var dev in devices)
                Console.WriteLine("{0} : {1}\n", idx++, dev.Description);

            Console.WriteLine("choose your index : ");
            int index = int.Parse(Console.ReadLine());

            using var device = CaptureDeviceList.Instance[index];
            device.OnPacketArrival += new PacketArrivalEventHandler(Device_OnPacketArrival);

            device.Open(DeviceModes.Promiscuous, 1000);
            device.Filter = "tcp    port 80 or 8080";
            
            device.StartCapture();
            
            Console.ReadLine();

            device.StopCapture();
        }
    }
}
