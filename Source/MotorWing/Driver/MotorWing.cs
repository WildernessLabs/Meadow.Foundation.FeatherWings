using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Hardware;
using System;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents Adafruit's Feather DC and Stepper Motor Wing
    /// </summary>
    public partial class MotorWing
    {
        readonly Pca9685 pca9685;

        /// <summary>
        /// Creates a MotorWing driver
        /// </summary>
        /// <param name="i2cBus">The I2C bus connected to the device</param>
        /// <param name="frequency">The PWM frequency for the PCA9685 IC</param>
        /// <param name="address">The address of the i2c Peripheral</param>
        public MotorWing(II2cBus i2cBus, Units.Frequency frequency, byte address = (byte)Pca9685.Addresses.Default)
        {
            if (i2cBus == null)
            {
                throw new ArgumentNullException("i2cBus");
            }

            pca9685 = new Pca9685(i2cBus, frequency, address);
        }

        /// <summary>
        /// Returns a instance of a Stepper Motor object
        /// </summary>
        /// <param name="steps">The number of steps the motor has</param>
        /// <param name="motorIndex">The stepper motor port</param>
        /// <returns>StepperMotor</returns>
        public StepperMotor GetStepper(StepperMotorIndex motorIndex, int steps)
        {
            return new StepperMotor(steps, motorIndex, pca9685);
        }

        /// <summary>
        /// Returns a instance of a DC Motor object
        /// </summary>
        /// <param name="motorIndex">The motor port</param>
        /// <returns>DCMotor</returns>
        public DCMotor GetMotor(DCMotorIndex motorIndex)
        {
            return new DCMotor(motorIndex, pca9685);
        }
    }
}