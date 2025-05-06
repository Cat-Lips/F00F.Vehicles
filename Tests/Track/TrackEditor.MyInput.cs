using System;
using Godot;

namespace Game;

public partial class TrackEditor
{
    static TrackEditor() => MyInput.Init();
    public static void Init() { }

    private class MyInput : F00F.MyInput
    {
        static MyInput() => Init<MyInput>();
        public static void Init() { }

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value
        public static readonly StringName Place;
        public static readonly StringName Remove;

        public static readonly StringName FlipFwd;
        public static readonly StringName FaceLeft;
        public static readonly StringName FaceRight;

        public static readonly StringName ScaleUp;
        public static readonly StringName ScaleDown;
        public static readonly StringName RotateLeft;
        public static readonly StringName RotateRight;

#pragma warning restore CS0649 // Field is never assigned to, and will always have its default value

        private static class Defaults
        {
            public static readonly Enum[] Place = [MouseButton.Left, Key.Enter, Key.KpEnter];
            public static readonly Enum[] Remove = [MouseButton.Left, Key.Enter, Key.KpEnter];

            public static readonly Enum[] FlipFwd = [MouseButton.Right];
            public static readonly Enum[] FaceLeft = [Key.Ctrl, MouseButton.Right];
            public static readonly Enum[] FaceRight = [Key.Alt, MouseButton.Right];

            public static readonly Enum[] ScaleUp = [MouseButton.WheelUp];
            public static readonly Enum[] ScaleDown = [MouseButton.WheelDown];
            public static readonly Enum[] RotateLeft = [Key.Ctrl, MouseButton.WheelUp];
            public static readonly Enum[] RotateRight = [Key.Alt, MouseButton.WheelDown];
        }
    }
}
