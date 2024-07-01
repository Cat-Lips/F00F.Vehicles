using Godot;

namespace F00F.Vehicles
{
    public partial class VehicleInput
    {
        private class MyInput : F00F.MyInput
        {
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
            public static readonly StringName Accelerate;
            public static readonly StringName Brake;
            public static readonly StringName SteerLeft;
            public static readonly StringName SteerRight;

            public static readonly StringName Turbo;  // (Run)
            public static readonly StringName Bounce; // (Jump)
            public static readonly StringName Drift;  // (handbrake)
            public static readonly StringName Reset;  // (flip upright)
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default values
            private static class Defaults
            {
                public static readonly Key[] Accelerate = [Key.W, Key.Up];
                public static readonly Key[] Brake = [Key.S, Key.Down];
                public static readonly Key[] SteerLeft = [Key.A, Key.Left];
                public static readonly Key[] SteerRight = [Key.D, Key.Right];

                public static readonly Key Turbo = Key.Shift;
                public static readonly Key Bounce = Key.Space;
                public static readonly Key Drift = Key.Ctrl;

                public static readonly Key[] Reset = [Key.F1, Key.Home];
            }

            static MyInput() => Init<MyInput>("Vehicle");
            private MyInput() { }
        }
    }
}
