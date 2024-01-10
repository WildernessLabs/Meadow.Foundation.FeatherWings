using Meadow.Foundation.Displays;
using Meadow.Foundation.Sensors.Buttons;
using Meadow.Hardware;
using System;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents Adafruit's 128 x 64 OLED Feather Wing
    /// </summary>
    public class OLED128x64Wing
    {
        /// <summary>
        /// Returns SH1107 object
        /// </summary>
        public Sh1107 Display { get; protected set; }

        /// <summary>
        /// Returns button A
        /// </summary>
        public PushButtonBase ButtonA { get; protected set; }

        /// <summary>
        /// Returns button B
        /// </summary>
        public PushButtonBase ButtonB { get; protected set; }

        /// <summary>
        /// Returns button C
        /// </summary>
        public PushButtonBase ButtonC { get; protected set; }

        /// <summary>
        /// Creates a OLED128x64Wing driver. Buttons will be <see cref="PushButton"/> if capable of the interrupt, or <see cref="PollingPushButton"/> otherwise.
        /// </summary>
        /// <param name="i2cBus">The I2C bus connected to the wing</param>
        /// <param name="pinA">The pin used for button A. Usually D11 for Feather format Meadow devices.</param>
        /// <param name="pinB">The pin used for button B. Usually D10 for Feather format Meadow devices.</param>
        /// <param name="pinC">The pin used for button C. Usually D09 for Feather format Meadow devices.</param>
        public OLED128x64Wing(II2cBus i2cBus, IPin pinA, IPin pinB, IPin pinC)
        {
            Display = new Sh1107(i2cBus, (byte)Sh1107.Addresses.Address_0x3C, width: 64, height: 128, firstColumn: 32);

            ButtonA = BestPossiblePushButton(pinA);
            ButtonB = BestPossiblePushButton(pinB);
            ButtonC = BestPossiblePushButton(pinC);
        }

        // TODO: A version of this method may want to be folded into Meadow.Foundation directly?
        /// <summary>
        /// Creates an interrupt-driven <see cref="PushButton"/> if possible, falling back to <see cref="PollingPushButton"/> if needed.
        /// </summary>
        /// <param name="pin">The pin connected to the button</param>
        /// <returns><see cref="PushButton"/> or <see cref="PollingPushButton"/> depending on the capabilities of the <paramref name="pin"/></returns>
        /// <remarks>This exists because the A, B, and C default pins on the Meadow F7 V1 and V2 differ in interrupt capabilities.</remarks>
        private PushButtonBase BestPossiblePushButton(IPin pin)
        {
            if (pin.Supports<IDigitalChannelInfo>(info => info.InterruptCapable))
            {
                return new PushButton(pin.CreateDigitalInterruptPort(InterruptMode.EdgeBoth, ResistorMode.InternalPullUp));
            }
            else
            {
                return new PollingPushButton(pin) { LongClickedThreshold = TimeSpan.Zero };
            }
        }
    }
}