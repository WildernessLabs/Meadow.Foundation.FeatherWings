using Meadow.Foundation.Displays;
using Meadow.Foundation.Sensors.Hid;
using Meadow.Foundation.Sensors.Light;
using Meadow.Hardware;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents Adafruits OLED Feather Wing
    /// </summary>
    public class KeyboardWing
    {
        /// <summary>
        /// Returns the Ili9341 driver
        /// </summary>
        public Ili9341 Display { get; protected set; }

        /// <summary>
        /// Returns the Tsc2004 driver
        /// </summary>
        public Tsc2004 TouchScreen { get; protected set; }

        /// <summary>
        /// Returns the BBQ10Keyboard driver
        /// </summary>
        public BBQ10Keyboard Keyboard { get; protected set; }

        /// <summary>
        /// Returns the LightSensor driver
        /// </summary>
        public AnalogLightSensor LightSensor { get; protected set; }

        /// <summary>
        /// Creates a KeyboardWing driver
        /// </summary>
        /// <param name="spiBus">The SPI bus used for the display</param>
        /// <param name="i2cBus">The I2C bus connected to the device</param>
        /// <param name="keyboardPin">The pin used to interrupt the keyboard</param>
        /// <param name="displayChipSelectPin">The chip select pin</param>
        /// <param name="displayDcPin">The DC pin</param>
        /// <param name="lightSensorPin">The light sensor pin</param>
        public KeyboardWing(
            ISpiBus spiBus,
            II2cBus i2cBus,
            IPin keyboardPin,
            IPin displayChipSelectPin,
            IPin displayDcPin,
            IPin lightSensorPin)
        {
            Keyboard = new BBQ10Keyboard(i2cBus, keyboardPin);

            TouchScreen = new Tsc2004(i2cBus)
            {
                DisplayHeight = 320,
                DisplayWidth = 240,
                XMin = 366,
                XMax = 3567,
                YMin = 334,
                YMax = 3787,
            };

            Display = new Ili9341
            (
                spiBus: spiBus,
                chipSelectPin: displayChipSelectPin, // Device.Pins.D11,
                dcPin: displayDcPin, // Device.Pins.D12,
                resetPin: null, //not used
                width: 240, height: 320
            );

            LightSensor = new AnalogLightSensor(lightSensorPin);
        }
    }
}