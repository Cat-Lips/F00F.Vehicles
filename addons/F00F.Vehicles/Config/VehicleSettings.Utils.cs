using System;
using Godot;

namespace F00F.Vehicles
{
    public partial class VehicleSettings
    {
        public bool IsSteerWheel(Node3D wheel) => Is(SteerType, wheel);
        public bool IsDriveWheel(Node3D wheel) => Is(DriveType, wheel);

        private static bool IsBack(Node3D wheel) => wheel.Position.Z > 0;
        private static bool IsFront(Node3D wheel) => wheel.Position.Z < 0;
        private static bool Is(WheelAction type, Node3D wheel) => type switch
        {
            WheelAction.All => true,
            WheelAction.Back => IsBack(wheel),
            WheelAction.Front => IsFront(wheel),
            _ => throw new NotImplementedException(),
        };
    }
}
