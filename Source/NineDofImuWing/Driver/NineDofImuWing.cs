using Meadow.Foundation.Sensors.Motion;
using Meadow.Hardware;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Motion;
using Meadow.Units;
using System;
using System.Threading.Tasks;

namespace Meadow.Foundation.FeatherWings
{
    /// <summary>
    /// Represents Adafruit's 9-DOF IMU FeatherWing
    /// </summary>
    public class NineDofImuWing :
        PollingSensorBase<(Acceleration3D? Acceleration3D, AngularVelocity3D? AngularVelocity3D, MagneticField3D? MagneticField3D)>,
        IAccelerometer, IGyroscope, IMagnetometer
    {
        /// <summary>
        /// The LIS3MDL Magnetometer
        /// </summary>
        public Lis3mdl Lis3mdl { get; private set; }

        /// <summary>
        /// The LSM6DSOX Accelerometer/Gyroscope
        /// </summary>
        public Lsm6dsox Lsm6dsox { get; private set; }

        /// <summary>
        /// The sampling interval
        /// </summary>
        public override TimeSpan UpdateInterval => ((ISamplingSensor<Acceleration3D>)Lsm6dsox).UpdateInterval;

        /// <summary>
        /// Is the sensor currently sampling
        /// </summary>
        public new bool IsSampling => Lsm6dsox.IsSampling && Lis3mdl.IsSampling;

        /// <summary>
        /// The current acceleration
        /// </summary>
        public Acceleration3D? Acceleration3D => ((IAccelerometer)Lsm6dsox).Acceleration3D;

        /// <summary>
        /// The current angular velocity
        /// </summary>
        public AngularVelocity3D? AngularVelocity3D => ((IGyroscope)Lsm6dsox).AngularVelocity3D;

        /// <summary>
        /// The current magnetic field
        /// </summary>
        public MagneticField3D? MagneticField3D => ((IMagnetometer)Lis3mdl).MagneticField3D;

        private event EventHandler<IChangeResult<Acceleration3D>>? acceleration3dHander;
        private event EventHandler<IChangeResult<AngularVelocity3D>>? angularVelocity3dHandler;
        private event EventHandler<IChangeResult<MagneticField3D>>? magneticField3dHander;

        event EventHandler<IChangeResult<Acceleration3D>> ISamplingSensor<Acceleration3D>.Updated
        {
            add => acceleration3dHander += value;
            remove => acceleration3dHander -= value;
        }

        event EventHandler<IChangeResult<AngularVelocity3D>> ISamplingSensor<AngularVelocity3D>.Updated
        {
            add => angularVelocity3dHandler += value;
            remove => angularVelocity3dHandler -= value;
        }

        event EventHandler<IChangeResult<MagneticField3D>> ISamplingSensor<MagneticField3D>.Updated
        {
            add => magneticField3dHander += value;
            remove => magneticField3dHander -= value;
        }

        /// <summary>
        /// Represents Adafruit's 9-DOF IMU FeatherWing with an <see cref="Lsm6dsox"/> Accelerometer/Gyroscope and an <see cref="Lis3mdl"/> Magnetometer.
        /// </summary>
        /// <param name="i2cBus">The I2C bus connected to the device</param>
        /// <param name="alternateAccelAddress">true if the solder jumper for A/G Address is closed on the back of the featherwing, or it's SDO pin is pulled high.</param>
        /// <param name="alternateMagAddress">true if the solder jumper for Mag Address is closed on the back of the featherwing, or it's SDO pin is pulled high.</param>
        /// <exception cref="ArgumentNullException">if <paramref name="i2cBus"/> is <c>null</c></exception>
        public NineDofImuWing(II2cBus i2cBus, bool alternateAccelAddress = false, bool alternateMagAddress = false)
        {
            if (i2cBus == null)
            {
                throw new ArgumentNullException(nameof(i2cBus));
            }

            Lsm6dsox = new Lsm6dsox(i2cBus, (byte)(alternateAccelAddress ? Lsm6dsox.Addresses.Address_0x6B : Lsm6dsox.Addresses.Default));
            Lis3mdl = new Lis3mdl(i2cBus, (byte)(alternateMagAddress ? Lis3mdl.Addresses.Address_0x1D : Lis3mdl.Addresses.Default));

            // Subscribe to lower level events so we can send Updated
            Lsm6dsox.Updated += Lsm6dsox_Updated;
            Lis3mdl.Updated += Lis3mdl_Updated;
        }

        private void Lsm6dsox_Updated(object sender, IChangeResult<(Acceleration3D? Acceleration3D, AngularVelocity3D? AngularVelocity3D)> e)
        {
            var oldValue = (e.Old?.Acceleration3D ?? null, e.Old?.AngularVelocity3D ?? null, MagneticField3D);
            var newValue = (e.New.Acceleration3D, e.New.AngularVelocity3D, MagneticField3D);
            var newChangeResult = new ChangeResult<(Acceleration3D? Acceleration3D, AngularVelocity3D? AngularVelocity3D, MagneticField3D? MagneticField3D)>(newValue, oldValue);
            RaiseEventsAndNotify(newChangeResult);
        }

        private void Lis3mdl_Updated(object sender, IChangeResult<MagneticField3D> e)
        {
            var oldValue = (Acceleration3D, AngularVelocity3D, e.Old ?? null);
            var newValue = (Acceleration3D, AngularVelocity3D, e.New);
            var newChangeResult = new ChangeResult<(Acceleration3D? Acceleration3D, AngularVelocity3D? AngularVelocity3D, MagneticField3D? MagneticField3D)>(newValue, oldValue);
            RaiseEventsAndNotify(newChangeResult);
        }

        /// <inheritdoc/>
        public override void StartUpdating(TimeSpan? updateInterval = null)
        {
            Lsm6dsox.StartUpdating(updateInterval);
            Lis3mdl.StartUpdating(updateInterval);
        }

        /// <inheritdoc/>
        public override void StopUpdating()
        {
            Lsm6dsox.StopUpdating();
            Lis3mdl.StopUpdating();
        }

        /// <inheritdoc/>
        protected override void RaiseEventsAndNotify(IChangeResult<(Acceleration3D? Acceleration3D, AngularVelocity3D? AngularVelocity3D, MagneticField3D? MagneticField3D)> changeResult)
        {
            if (changeResult.New.Acceleration3D is { } a3d)
            {
                acceleration3dHander?.Invoke(this, new ChangeResult<Acceleration3D>(a3d, changeResult.Old?.Acceleration3D));
            }

            if (changeResult.New.AngularVelocity3D is { } av3d)
            {
                angularVelocity3dHandler?.Invoke(this, new ChangeResult<AngularVelocity3D>(av3d, changeResult.Old?.AngularVelocity3D));
            }

            if (changeResult.New.MagneticField3D is { } m3d)
            {
                magneticField3dHander?.Invoke(this, new ChangeResult<MagneticField3D>(m3d, changeResult.Old?.MagneticField3D));
            }

            base.RaiseEventsAndNotify(changeResult);
        }

        /// <inheritdoc/>
        protected override Task<(Acceleration3D? Acceleration3D, AngularVelocity3D? AngularVelocity3D, MagneticField3D? MagneticField3D)> ReadSensor()
        {
            (Acceleration3D? Acceleration3D, AngularVelocity3D? AngularVelocity3D, MagneticField3D? MagneticField3D) conditions;

            var sensor1 = Lsm6dsox.Read().Result;
            var sensor2 = Lis3mdl.Read().Result;

            conditions.Acceleration3D = sensor1.Acceleration3D;
            conditions.AngularVelocity3D = sensor1.AngularVelocity3D;
            conditions.MagneticField3D = sensor2;

            return Task.FromResult(conditions);
        }

        Task<Acceleration3D> ISensor<Acceleration3D>.Read() => ((ISensor<Acceleration3D>)Lsm6dsox).Read();
        Task<AngularVelocity3D> ISensor<AngularVelocity3D>.Read() => ((ISensor<AngularVelocity3D>)Lsm6dsox).Read();
        Task<MagneticField3D> ISensor<MagneticField3D>.Read() => ((ISensor<MagneticField3D>)Lis3mdl).Read();
    }
}