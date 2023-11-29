using Meadow;
using Meadow.Devices;
using Meadow.Foundation.FeatherWings;
using Meadow.Foundation.Graphics;
using System;
using System.Threading.Tasks;

namespace FeatherWings.NeoPixel_Sample
{
    public class MeadowApp : App<F7FeatherV2>
    {
        //<!=SNIP=>

        NeoPixelWing neoWing;
        MicroGraphics graphics;

        public override Task Initialize()
        {
            Console.WriteLine("Initializing ...");

            neoWing = new NeoPixelWing(Device.CreateSpiBus());

            graphics = new MicroGraphics(neoWing);

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            graphics.Clear();

            graphics.DrawRectangle(0, 0, 8, 4, false);

            graphics.Show();

            return Task.CompletedTask;
        }

        //<!=SNOP=>
    }
}