using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Hardware;
using static Meadow.Foundation.FeatherWings.MotorWing;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents a DC Motor
    /// </summary>
    public class DCMotor
    {
        private readonly IPwmPort pwmPort;
        private readonly IDigitalOutputPort in1;
        private readonly IDigitalOutputPort in2;

        readonly Pca9685 controller;

        /// <summary>
        /// Creates a DCMotor driver
        /// </summary>
        public DCMotor(DCMotorIndex motorIndex, Pca9685 pca9685)
        {
            controller = pca9685;

            switch (motorIndex)
            {
                case DCMotorIndex.Motor1:
                    pwmPort = controller.Pins.LED8.CreatePwmPort(pca9685.Frequency);
                    in1 = controller.Pins.LED10.CreateDigitalOutputPort();
                    in2 = controller.Pins.LED9.CreateDigitalOutputPort();
                    break;
                case DCMotorIndex.Motor2:
                    pwmPort = controller.Pins.LED13.CreatePwmPort(pca9685.Frequency);
                    in1 = controller.Pins.LED11.CreateDigitalOutputPort();
                    in2 = controller.Pins.LED12.CreateDigitalOutputPort();
                    break;
                case DCMotorIndex.Motor3:
                    pwmPort = controller.Pins.LED2.CreatePwmPort(pca9685.Frequency);
                    in1 = controller.Pins.LED4.CreateDigitalOutputPort();
                    in2 = controller.Pins.LED3.CreateDigitalOutputPort();
                    break;
                case DCMotorIndex.Motor4:
                    pwmPort = controller.Pins.LED7.CreatePwmPort(pca9685.Frequency);
                    in1 = controller.Pins.LED5.CreateDigitalOutputPort();
                    in2 = controller.Pins.LED6.CreateDigitalOutputPort();
                    break;
            }

            Run(Direction.Release);
        }

        /// <summary>
        /// Controls the motor direction/action
        /// </summary>
        /// <param name="command">The action</param>
        public virtual void Run(Direction command)
        {
            if (command == Direction.Forward)
            {
                in1.State = true;
                in2.State = false;
            }
            else if (command == Direction.Reverse)
            {
                in1.State = false;
                in2.State = true;
            }

            if (command == Direction.Release)
            {
                in1.State = false;
                in2.State = false;
            }
        }

        /// <summary>
        /// Control the DC Motor speed/throttle
        /// </summary>
        /// <param name="speed">0 is off, 1 is on</param>
        public void SetSpeed(double speed)
        {
            pwmPort.DutyCycle = speed;
        }

        /// <summary>
        /// Stops the motor
        /// </summary>
        public void Stop()
        {
            Run(Direction.Release);
            pwmPort.DutyCycle = 0;
        }
    }
}