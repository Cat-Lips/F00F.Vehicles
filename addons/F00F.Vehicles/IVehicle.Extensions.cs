using System.Linq;
using Godot;

namespace F00F;

public static class IVehicleExtensions
{
    public static Node3D[] Wheels<T>(this T vehicle) where T : IVehicle
        => (vehicle as Node).GetChildren<Node3D>().Where(vehicle.Model.IsWheel).ToArray();

    public static Node3D[] Wheels<T>(this T vehicle, Node3D[] wheels) where T : IVehicle
        => wheels ?? vehicle.Wheels();

    public static Node3D[] SteerWheels<T>(this T vehicle, Node3D[] wheels) where T : IVehicle
        => vehicle.Wheels(wheels).Where(vehicle.Config.IsSteerWheel).ToArray();

    public static Node3D[] DriveWheels<T>(this T vehicle, Node3D[] wheels) where T : IVehicle
        => vehicle.Wheels(wheels).Where(vehicle.Config.IsDriveWheel).ToArray();

    public static Vector3 SteerAxle<T>(this T vehicle, Node3D[] wheels = null) where T : IVehicle
        => vehicle.SteerWheels(wheels).AverageOrDefault(x => x.Position);

    public static Vector3 DriveAxle<T>(this T vehicle, Node3D[] wheels = null) where T : IVehicle
        => vehicle.DriveWheels(wheels).AverageOrDefault(x => x.Position);

    public static Vector3 WheelCenter<T>(this T vehicle, Node3D[] wheels = null) where T : IVehicle
        => vehicle.Wheels(wheels).AverageOrDefault(x => x.Position);

    public static float WheelSpan<T>(this T vehicle, Node3D[] wheels = null) where T : IVehicle
        => vehicle.DriveAxle(wheels).DistanceTo(vehicle.SteerAxle(wheels));
}
