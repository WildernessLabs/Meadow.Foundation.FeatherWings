# Meadow.Foundation.FeatherWings.NineDofImuWing

**AdaFruit I2C ST 9-DoF IMU FeatherWing**

The **NineDofImuWing** library is designed for the [Wilderness Labs](www.wildernesslabs.co) Meadow .NET IoT platform and is part of [Meadow.Foundation](https://developer.wildernesslabs.co/Meadow/Meadow.Foundation/).

The **Meadow.Foundation** peripherals library is an open-source repository of drivers and libraries that streamline and simplify adding hardware to your C# .NET Meadow IoT application.

For more information on developing for Meadow, visit [developer.wildernesslabs.co](http://developer.wildernesslabs.co/).

To view all Wilderness Labs open-source projects, including samples, visit [github.com/wildernesslabs](https://github.com/wildernesslabs/).

## Usage

```csharp
NineDofImuWing nineDofImuWing;

public override Task Initialize()
{
    Resolver.Log.Info("Initializing ...");

    var i2cBus = Device.CreateI2cBus(I2cBusSpeed.FastPlus);

    nineDofImuWing = new NineDofImuWing(i2cBus);

    // Example that uses an IObservable subscription to only be notified when the filter is satisfied
    var consumer = NineDofImuWing.CreateObserver(handler: result => HandleResult(this, result),
                                         filter: result => FilterResult(result));

    nineDofImuWing.Subscribe(consumer);

    // classical .NET events can also be used for all sensors
    nineDofImuWing.Updated += HandleResult;

    nineDofImuWing.StartUpdating(TimeSpan.FromSeconds(1));

    return Task.CompletedTask;
}

bool FilterResult(IChangeResult<(
        Acceleration3D? Acceleration3D,
        AngularVelocity3D? AngularVelocity3D,
        MagneticField3D? MagneticField3D
    )> result)
{
    return result.New.Acceleration3D.Value.Z > new Acceleration(0.1, Acceleration.UnitType.Gravity);
}

// Example Event Handler for all sensor results
void HandleResult(object sender, IChangeResult<(
        Acceleration3D? Acceleration3D,
        AngularVelocity3D? AngularVelocity3D,
        MagneticField3D? MagneticField3D
    )> result)
{
    var accel = result.New.Acceleration3D.Value;
    var gyro = result.New.AngularVelocity3D.Value;
    var mag = result.New.MagneticField3D.Value;

    Resolver.Log.Info(
        $"Accelerometer (g): X = {accel.X.Gravity:0.##}, Y = {accel.Y.Gravity:0.##}, Z = {accel.Z.Gravity:0.##}; " +
        $"Gyroscope (°/s): X = {gyro.X.DegreesPerSecond:0.##}, Y = {gyro.Y.DegreesPerSecond:0.##}, Z = {gyro.Z.DegreesPerSecond:0.##}; " +
        $"Magnetometer (gauss) X = {mag.X.Gauss:0.##}, Y = {mag.Y.Gauss:0.##}, Z = {mag.Z.Gauss:0.##}"
        );
}

// Example Event Handler for any individual sensors' result
void HandleResult<UNIT>(object sender, IChangeResult<UNIT> result)
where UNIT : struct
{
    if (result is IChangeResult<Acceleration3D> accel)
    {
        Resolver.Log.Info($"Accelerometer (g): X = {accel.New.X.Gravity:0.##}, Y = {accel.New.Y.Gravity:0.##}, Z = {accel.New.Z.Gravity:0.##}");
    }
    else if (result is IChangeResult<AngularVelocity3D> gyro)
    {
        Resolver.Log.Info($"Gyroscope (°/s): X = {gyro.New.X.DegreesPerSecond:0.##}, Y = {gyro.New.Y.DegreesPerSecond:0.##}, Z = {gyro.New.Z.DegreesPerSecond:0.##};");
    }
    else if (result is IChangeResult<MagneticField3D> mag)
    {
        Resolver.Log.Info($"Magnetometer (gauss) X = {mag.New.X.Gauss:0.##}, Y = {mag.New.Y.Gauss:0.##}, Z = {mag.New.Z.Gauss:0.##}");
    }
}

```
## How to Contribute

- **Found a bug?** [Report an issue](https://github.com/WildernessLabs/Meadow_Issues/issues)
- Have a **feature idea or driver request?** [Open a new feature request](https://github.com/WildernessLabs/Meadow_Issues/issues)
- Want to **contribute code?** Fork the [Meadow.Foundation.Featherwings](https://github.com/WildernessLabs/Meadow.Foundation.Featherwings) repository and submit a pull request against the `develop` branch


## Need Help?

If you have questions or need assistance, please join the Wilderness Labs [community on Slack](http://slackinvite.wildernesslabs.co/).
