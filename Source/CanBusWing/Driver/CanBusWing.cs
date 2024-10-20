using Meadow.Foundation.ICs.CAN;
using Meadow.Hardware;
using Meadow.Units;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents an Adafruit CAN Bus featherwing
    /// </summary>
    public class CanBusWing : Mcp2515
    {
        /// <summary>
        /// Creates a CanBusWing driver
        /// </summary>
        public CanBusWing(IF7FeatherMeadowDevice feather)
            : base(
                  bus: feather.CreateSpiBus(3, 1_000_000.Hertz()),
                  chipSelect: feather.Pins.D09.CreateDigitalOutputPort(true),
                  interruptPort: feather.Pins.D10.CreateDigitalInterruptPort(InterruptMode.EdgeFalling),
                  oscillator: CanOscillator.Osc_16MHz)
        { }
    }
}