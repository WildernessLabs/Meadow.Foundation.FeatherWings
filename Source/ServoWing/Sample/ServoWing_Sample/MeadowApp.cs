using Meadow;
using Meadow.Devices;
using Meadow.Foundation.FeatherWings;
using Meadow.Foundation.Servos;
using Meadow.Hardware;
using Meadow.Units;
using System;
using System.Threading.Tasks;
using AU = Meadow.Units.Angle.UnitType;

namespace FeatherWings.ServoWing_Sample
{
    public class MeadowApp : App<F7FeatherV2>
    {
        //<!=SNIP=>

        ServoWing servoWing;
        AngularServo servo;

        public override Task Initialize()
        {
            Console.WriteLine("Initializng ...");

            servoWing = new ServoWing(Device.CreateI2cBus(I2cBusSpeed.FastPlus));

            servo = servoWing.GetServo(0,
                new AngularServo.PulseAngle(NamedServoConfigs.SG90.MinimumAngle, new TimePeriod(NamedServoConfigs.SG90.MinimumPulseDuration, TimePeriod.UnitType.Milliseconds)),
                new AngularServo.PulseAngle(NamedServoConfigs.SG90.MaximumAngle, new TimePeriod(NamedServoConfigs.SG90.MaximumPulseDuration, TimePeriod.UnitType.Milliseconds)));

            return Task.CompletedTask;
        }

        public override async Task Run()
        {
            while (true)
            {
                Console.WriteLine("0");
                servo.RotateTo(new Angle(0, AU.Degrees));
                await Task.Delay(1000);

                Console.WriteLine("45");
                servo.RotateTo(new Angle(45, AU.Degrees));
                await Task.Delay(1000);

                Console.WriteLine("90");
                servo.RotateTo(new Angle(90, AU.Degrees));
                await Task.Delay(1000);

                Console.WriteLine("135");
                servo.RotateTo(new Angle(135, AU.Degrees));
                await Task.Delay(1000);
            }
        }

        //<!=SNOP=>
    }
}