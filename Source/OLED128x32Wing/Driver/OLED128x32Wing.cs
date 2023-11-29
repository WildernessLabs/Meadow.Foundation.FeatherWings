using Meadow.Foundation.Displays;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents Adafruits OLED Feather Wing
    /// </summary>
    public class OLED128x32Wing
    {
        /// <summary>
        /// Returns Ssd1306 object
        /// </summary>
        public Ssd1306 Display { get; protected set; }

        /// <summary>
        /// Returns button A
        /// </summary>
        public PushButton ButtonA { get; protected set; }

        /// <summary>
        /// Returns button B
        /// </summary>
        public PushButton ButtonB { get; protected set; }

        /// <summary>
        /// Returns button C
        /// </summary>
        public PushButton ButtonC { get; protected set; }

        /// <summary>
        /// Creates a OLED128x32Wing driver
        /// </summary>
        /// <param name="i2cBus">The I2C bus connected to the wing</param>
        /// <param name="portA">The digital port for button A</param>
        /// <param name="portB">The digital port for button B</param>
        /// <param name="portC">The digital port for button C</param>
        public OLED128x32Wing(II2cBus i2cBus, IDigitalInterruptPort portA, IDigitalInterruptPort portB, IDigitalInterruptPort portC)
        {
            Display = new Ssd1306(i2cBus, (byte)Ssd1306.Addresses.Default, Ssd1306.DisplayType.OLED128x32);

            ButtonA = new PushButton(portA);
            ButtonB = new PushButton(portB);
            ButtonC = new PushButton(portC);
        }

        /// <summary>
        /// Creates a OLED128x32Wing driver
        /// </summary>
        /// <param name="i2cBus">The I2C bus connected to the wing</param>
        /// <param name="pinA">The pin used for button A</param>
        /// <param name="pinB">The pin used for button B</param>
        /// <param name="pinC">The pin used for button C</param>
        public OLED128x32Wing(II2cBus i2cBus, IPin pinA, IPin pinB, IPin pinC) :
            this(i2cBus,
                pinA.CreateDigitalInterruptPort(InterruptMode.EdgeBoth, ResistorMode.InternalPullUp),
                pinB.CreateDigitalInterruptPort(InterruptMode.EdgeBoth, ResistorMode.InternalPullUp),
                pinC.CreateDigitalInterruptPort(InterruptMode.EdgeBoth, ResistorMode.InternalPullUp))
        { }
    }
}