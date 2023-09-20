﻿using Meadow;
using Meadow.Devices;
using Meadow.Foundation.FeatherWings;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace FeatherWings.MotorWing_Sample
{
    public class MeadowApp : App<F7FeatherV2>
    {
        //<!=SNIP=>

        MotorWing motorWing;

        public override Task Initialize()
        {
            Console.WriteLine("Initializing ...");

            var i2CBus = Device.CreateI2cBus(I2cBusSpeed.FastPlus);
            motorWing = new MotorWing(i2CBus, new Frequency(100, Frequency.UnitType.Hertz), 0x61);
            motorWing.Initialize();

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            //Get DC motor 1
            var dcMotor1 = motorWing.GetMotor(1);

            //Get DC motor 2
            var dcMotor2 = motorWing.GetMotor(2);

            //Get Stepper motor number 2
            var stepper = motorWing.GetStepper(2, 200);

            dcMotor1.Run(Command.FORWARD);
            dcMotor2.Run(Command.BACKWARD);

            while (true)
            {
                Console.WriteLine("Speed up");
                for (short i = 0; i <= 255; i++)
                {
                    dcMotor1.SetSpeed(i);
                    dcMotor2.SetSpeed(i);
                    await Task.Delay(10);
                }

                stepper.Step(50);

                await Task.Delay(500);

                Console.WriteLine("Slow down");
                for (short i = 255; i >= 0; i--)
                {
                    dcMotor1.SetSpeed(i);
                    dcMotor2.SetSpeed(i);
                    await Task.Delay(10);
                }

                stepper.Step(-50);

                await Task.Delay(500);
            }
        }

        //<!=SNOP=>
    }
}