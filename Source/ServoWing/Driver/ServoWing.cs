using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Foundation.Servos;
using Meadow.Hardware;
using Meadow.Peripherals.Servos;
using Meadow.Units;
using System;
using static Meadow.Foundation.Servos.AngularServo;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents Adafruit's Feather Servo Wing and 16-Channel 12-bit PWM/Servo Shield
    /// </summary>
    /// <remarks>All PWM channels run at the same Frequency</remarks>
    public class ServoWing
    {
        readonly Pca9685 pca9685;

        readonly short portCount;

        /// <summary>
        /// Creates a ServoWing driver
        /// </summary>
        /// <param name="i2cBus"></param>
        /// <param name="address"></param>
        /// <param name="portCount"></param>
        public ServoWing(II2cBus i2cBus, byte address = (byte)Pca9685.Addresses.Default, short portCount = 8)
        : this(i2cBus, new Frequency(IPwmOutputController.DefaultPwmFrequency, Frequency.UnitType.Hertz), address, portCount)
        { }

        /// <summary>
        /// Creates a ServoWing driver
        /// </summary>
        /// <param name="i2cBus"></param>
        /// <param name="address"></param>
        /// <param name="frequency"></param>
        /// <param name="portCount"></param>
        public ServoWing(
            II2cBus i2cBus,
            Frequency frequency,
            byte address = (byte)Pca9685.Addresses.Default,
            short portCount = 8)
        {
            if (portCount != 8 && portCount != 16)
            {
                throw new ArgumentException("Channels need to be 8 or 16", "ports");
            }

            this.portCount = portCount;
            pca9685 = new Pca9685(i2cBus, frequency, address);
        }

        /// <summary>
        /// Returns the specified servo
        /// </summary>
        /// <param name="portIndex"></param>
        /// <param name="minPulseAngle">The pulse angle corresponding to the minimum angle of the servo.</param>
        /// <param name="maxPulseAngle">The pulse angle corresponding to the maximum angle of the servo.</param>
        public AngularServo GetServo(byte portIndex, PulseAngle minPulseAngle, PulseAngle maxPulseAngle)
        {
            if ((portIndex < 0) || (portIndex > portCount))
            {
                throw new ArgumentException($"Servo num must be between 1 and {portCount}", "num");
            }

            var pwm = pca9685.CreatePwmPort(GetPinForPortIndex(portIndex));
            var servo = new AngularServo(pwm, minPulseAngle, maxPulseAngle);

            return servo;
        }

        /// <summary>
        /// Returns the specified continues rotation servo
        /// </summary>
        /// <param name="portIndex"></param>
        /// <param name="minimumPulseDuration">The minimum pulse duration for the servo.</param>
        /// <param name="maximumPulseDuration">The maximum pulse duration for the servo.</param>
        public IContinuousRotationServo GetContinuousRotatioServo(byte portIndex, TimePeriod minimumPulseDuration, TimePeriod maximumPulseDuration)
        {
            var pin = GetPinForPortIndex(portIndex);

            var pwm = pca9685.CreatePwmPort(pin);
            var servo = new ContinuousRotationServo(pwm, minimumPulseDuration, maximumPulseDuration);

            return servo;
        }

        private IPin? GetPinForPortIndex(byte portIndex)
        {
            foreach (var pin in pca9685.Pins)
            {
                if ((byte)pin.Key == portIndex)
                {
                    return pin;
                }
            }

            return null;
        }
    }
}