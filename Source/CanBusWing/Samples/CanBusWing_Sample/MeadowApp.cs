using Meadow;
using Meadow.Devices;
using Meadow.Foundation.FeatherWings;
using Meadow.Hardware;
using System;
using System.Threading.Tasks;

namespace MeadowApp
{
    public class MeadowApp : App<F7FeatherV2>
    {
        //<!=SNIP=>

        private CanBusWing wing;

        public override Task Initialize()
        {
            Console.WriteLine("Initialize...");

            wing = new CanBusWing(Device);

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            var bus = wing.CreateCanBus(CanBitrate.Can_250kbps);

            Console.WriteLine($"Listening for CAN data...");

            var tick = 0;

            while (true)
            {
                var frame = bus.ReadFrame();
                if (frame != null)
                {
                    if (frame is StandardDataFrame sdf)
                    {
                        Console.WriteLine($"Standard Frame: {sdf.ID:X3} {BitConverter.ToString(sdf.Payload)}");
                    }
                    else if (frame is ExtendedDataFrame edf)
                    {
                        Console.WriteLine($"Extended Frame: {edf.ID:X8} {BitConverter.ToString(edf.Payload)}");
                    }
                }
                else
                {
                    await Task.Delay(100);
                }

                if (tick++ % 50 == 0)
                {
                    Console.WriteLine($"Sending Standard Frame...");

                    bus.WriteFrame(new StandardDataFrame
                    {
                        ID = 0x700,
                        Payload = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, (byte)(tick & 0xff) }
                    });
                }
            }
        }

        //<!=SNOP=>
    }
}