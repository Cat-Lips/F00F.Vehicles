using Godot;

namespace F00F.Vehicles.Tests
{
    public partial class Main
    {
        private class MyInput : F00F.MyInput
        {
#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
            public static readonly StringName Quit;
            public static readonly StringName ToggleFloor;
            public static readonly StringName ToggleTarget;
#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value
            internal static class Defaults
            {
                public static readonly Key Quit = Key.End;
                public static readonly Key ToggleFloor = Key.F12;
                public static readonly Key ToggleTarget = Key.F11;
            }

            static MyInput() => Init<MyInput>();
            private MyInput() { }
        }
    }
}
