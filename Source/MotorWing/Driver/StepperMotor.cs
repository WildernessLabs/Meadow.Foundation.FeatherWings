using Meadow.Foundation.ICs.IOExpanders;
using Meadow.Hardware;
using System;
using System.Threading;
using static Meadow.Foundation.FeatherWings.MotorWing;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents a Stepper Motor and provides functionality to control it.
    /// </summary>
    public class StepperMotor
    {
        private readonly IPwmPort portA;
        private readonly IPwmPort portB;

        private int currentStep;
        private double rpmDelay;
        private readonly int motorSteps;

        private readonly IPwmPort pwmPortA;
        private readonly IDigitalOutputPort in1A;
        private readonly IDigitalOutputPort in2A;

        private readonly IPwmPort pwmPortB;
        private readonly IDigitalOutputPort in1B;
        private readonly IDigitalOutputPort in2B;

        private const short MICROSTEPS = 8;
        private readonly byte[] microStepCurve = { 0, 50, 98, 142, 180, 212, 236, 250, 255 };

        /// <summary>
        /// Initializes a new instance of the <see cref="StepperMotor"/> class.
        /// </summary>
        /// <param name="steps">The number of steps per revolution.</param>
        /// <param name="motorIndex">The index of the stepper motor port.</param>
        /// <param name="pca9685">The PCA9685 driver object.</param>
        /// <exception cref="ArgumentException">Thrown when an invalid stepper motor index is provided.</exception>
        public StepperMotor(int steps, StepperMotorIndex motorIndex, Pca9685 pca9685)
        {
            if (motorIndex == StepperMotorIndex.Motor1)
            {
                pwmPortA = pca9685.Pins.LED8.CreatePwmPort(pca9685.Frequency);
                in1A = pca9685.Pins.LED10.CreateDigitalOutputPort();
                in2A = pca9685.Pins.LED9.CreateDigitalOutputPort();

                pwmPortB = pca9685.Pins.LED13.CreatePwmPort(pca9685.Frequency);
                in1B = pca9685.Pins.LED11.CreateDigitalOutputPort();
                in2B = pca9685.Pins.LED12.CreateDigitalOutputPort();
            }
            else if (motorIndex == StepperMotorIndex.Motor2)
            {
                pwmPortA = pca9685.Pins.LED2.CreatePwmPort(pca9685.Frequency);
                in1A = pca9685.Pins.LED4.CreateDigitalOutputPort();
                in2A = pca9685.Pins.LED3.CreateDigitalOutputPort();

                pwmPortB = pca9685.Pins.LED7.CreatePwmPort(pca9685.Frequency);
                in1B = pca9685.Pins.LED5.CreateDigitalOutputPort();
                in2B = pca9685.Pins.LED6.CreateDigitalOutputPort();
            }
            else
            {
                throw new ArgumentException("Stepper motor index must be Motor1 or Motor2");
            }

            motorSteps = steps;
            SetSpeed(15);
            currentStep = 0;
        }

        /// <summary>
        /// Sets the speed of the stepper motor in revolutions per minute (RPM).
        /// </summary>
        /// <param name="rpm">The desired RPM.</param>
        public void SetSpeed(short rpm)
        {
            rpmDelay = 60000.0 / (motorSteps * rpm);
        }

        /// <summary>
        /// Moves the stepper motor a specified number of steps at the current speed.
        /// </summary>
        /// <param name="steps">The number of steps to move. A negative value moves the stepper backwards.</param>
        /// <param name="style">The style of stepping to use.</param>
        public virtual void Step(int steps = 1, Style style = Style.SINGLE)
        {
            if (steps > 0)
            {
                Step(steps, Direction.Forward, style);
            }
            else
            {
                Step(Math.Abs(steps), Direction.Reverse, style);
            }
        }

        /// <summary>
        /// Moves the stepper motor a specified number of steps in a given direction and style.
        /// </summary>
        /// <param name="steps">The number of steps to move.</param>
        /// <param name="direction">The direction to move.</param>
        /// <param name="style">The style of stepping to use.</param>
        protected virtual void Step(int steps, Direction direction, Style style)
        {
            int delay = (int)rpmDelay;
            if (style == Style.INTERLEAVE)
            {
                delay /= 2;
            }
            else if (style == Style.MICROSTEP)
            {
                delay /= MICROSTEPS;
                steps *= MICROSTEPS;
            }

            while (steps >= 0)
            {
                Step(direction, style);
                Thread.Sleep(delay);
                steps--;
            }
        }

        /// <summary>
        /// Moves the stepper motor one step in a given direction and style.
        /// </summary>
        /// <param name="direction">The direction to move.</param>
        /// <param name="style">The style of stepping to use.</param>
        /// <returns>The current step position.</returns>
        protected virtual int Step(Direction direction, Style style)
        {
            int ocrb, ocra;
            ocra = ocrb = 255;

            if (style == Style.SINGLE)
            {
                if (currentStep / (MICROSTEPS / 2) % 2 != 0) // Odd step
                {
                    if (direction == Direction.Forward)
                    {
                        currentStep += MICROSTEPS / 2;
                    }
                    else
                    {
                        currentStep -= MICROSTEPS / 2;
                    }
                }
                else // Even step
                {
                    if (direction == Direction.Forward)
                    {
                        currentStep += MICROSTEPS;
                    }
                    else
                    {
                        currentStep -= MICROSTEPS;
                    }
                }
            }
            else if (style == Style.DOUBLE)
            {
                if (currentStep / (MICROSTEPS / 2) % 2 != 0) // Even step
                {
                    if (direction == Direction.Forward)
                    {
                        currentStep += MICROSTEPS / 2;
                    }
                    else
                    {
                        currentStep -= MICROSTEPS / 2;
                    }
                }
                else // Odd step
                {
                    if (direction == Direction.Forward)
                    {
                        currentStep += MICROSTEPS;
                    }
                    else
                    {
                        currentStep -= MICROSTEPS;
                    }
                }
            }
            else if (style == Style.INTERLEAVE)
            {
                if (direction == Direction.Forward)
                {
                    currentStep += MICROSTEPS / 2;
                }
                else
                {
                    currentStep -= MICROSTEPS / 2;
                }
            }
            else if (style == Style.MICROSTEP)
            {
                if (direction == Direction.Forward)
                {
                    currentStep++;
                }
                else
                {
                    currentStep--;
                }

                currentStep += MICROSTEPS * 4;
                currentStep %= MICROSTEPS * 4;
                ocra = ocrb = 0;

                if (currentStep < MICROSTEPS)
                {
                    ocra = microStepCurve[MICROSTEPS - currentStep];
                    ocrb = microStepCurve[currentStep];
                }
                else if (currentStep < MICROSTEPS * 2)
                {
                    ocra = microStepCurve[currentStep - MICROSTEPS];
                    ocrb = microStepCurve[MICROSTEPS * 2 - currentStep];
                }
                else if (currentStep < MICROSTEPS * 3)
                {
                    ocra = microStepCurve[MICROSTEPS * 3 - currentStep];
                    ocrb = microStepCurve[currentStep - MICROSTEPS * 2];
                }
                else
                {
                    ocra = microStepCurve[currentStep - MICROSTEPS * 3];
                    ocrb = microStepCurve[MICROSTEPS * 4 - currentStep];
                }
            }

            currentStep += MICROSTEPS * 4;
            currentStep %= MICROSTEPS * 4;

            pwmPortA.DutyCycle = ocra / 255.0;
            pwmPortB.DutyCycle = ocrb / 255.0;

            int latchState = 0;

            if (style == Style.MICROSTEP)
            {
                if (currentStep < MICROSTEPS)
                    latchState |= 0x03;
                else if (currentStep < MICROSTEPS * 2)
                    latchState |= 0x06;
                else if (currentStep < MICROSTEPS * 3)
                    latchState |= 0x0C;
                else
                    latchState |= 0x09;
            }
            else
            {
                switch (currentStep / (MICROSTEPS / 2))
                {
                    case 0:
                        latchState |= 0x1;
                        break;
                    case 1:
                        latchState |= 0x3;
                        break;
                    case 2:
                        latchState |= 0x2;
                        break;
                    case 3:
                        latchState |= 0x6;
                        break;
                    case 4:
                        latchState |= 0x4;
                        break;
                    case 5:
                        latchState |= 0xC;
                        break;
                    case 6:
                        latchState |= 0x8;
                        break;
                    case 7:
                        latchState |= 0x9;
                        break;
                }
            }

            in1A.State = (latchState & 0x1) != 0;
            in1B.State = (latchState & 0x2) != 0;
            in2A.State = (latchState & 0x4) != 0;
            in2B.State = (latchState & 0x8) != 0;

            return currentStep;
        }
    }
}
