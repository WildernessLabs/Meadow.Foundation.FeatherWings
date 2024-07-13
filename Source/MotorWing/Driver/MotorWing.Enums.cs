namespace Meadow.Foundation.FeatherWings
{
    public partial class MotorWing
    {
        /// <summary>
        /// Represents the index of DC motors on the MotorWing.
        /// </summary>
        public enum DCMotorIndex : byte
        {
            /// <summary>
            /// Represents DC Motor 1.
            /// </summary>
            Motor1 = 0,
            /// <summary>
            /// Represents DC Motor 2.
            /// </summary>
            Motor2 = 1,
            /// <summary>
            /// Represents DC Motor 3.
            /// </summary>
            Motor3 = 2,
            /// <summary>
            /// Represents DC Motor 4.
            /// </summary>
            Motor4 = 3
        }

        /// <summary>
        /// Represents the index of stepper motors on the MotorWing.
        /// </summary>
        public enum StepperMotorIndex : byte
        {
            /// <summary>
            /// Represents Stepper Motor 1.
            /// </summary>
            Motor1 = 0,
            /// <summary>
            /// Represents Stepper Motor 2.
            /// </summary>
            Motor2 = 1
        }

        /// <summary>
        /// Defines the different motor styles for stepper motors.
        /// </summary>
        public enum Style
        {
            /// <summary>
            /// Single step mode. Moves one step at a time.
            /// </summary>
            SINGLE = 1,
            /// <summary>
            /// Double step mode. Moves two steps at a time.
            /// </summary>
            DOUBLE = 2,

            /// <summary>
            /// Interleaved mode. Alternates between single and double steps.
            /// </summary>
            INTERLEAVE = 3,
            /// <summary>
            /// Microstep mode. Moves in smaller steps for smoother motion.
            /// </summary>
            MICROSTEP = 4
        }

        /// <summary>
        /// Defines the direction of motor movement.
        /// </summary>
        public enum Direction
        {
            /// <summary>
            /// Moves the motor forward.
            /// </summary>
            Forward,
            /// <summary>
            /// Moves the motor in reverse (backward).
            /// </summary>
            Reverse,
            /// <summary>
            /// Releases the motor. Applies only to stepper motors.
            /// </summary>
            Release
        }
    }
}