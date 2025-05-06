using Godot;

namespace F00F;

public partial class VehicleInput
{
    static VehicleInput() => MyInput.Init();

    private class MyInput : F00F.MyInput
    {
        static MyInput() => Init<MyInput>();
        public static void Init() { }

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
        public static readonly StringName Forward;
        public static readonly StringName Reverse;
        public static readonly StringName SteerLeft;
        public static readonly StringName SteerRight;

        public static readonly StringName Brake;
        public static readonly StringName Jump;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

        private static class Defaults
        {
            public static readonly Key[] Forward = [Key.W, Key.Up];
            public static readonly Key[] Reverse = [Key.S, Key.Down];
            public static readonly Key[] SteerLeft = [Key.A, Key.Left];
            public static readonly Key[] SteerRight = [Key.D, Key.Right];

            public static readonly Key Brake = Key.Space;
            public static readonly Key Jump = Key.Ctrl;
        }
    }
}
