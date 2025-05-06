using System;
using Godot;

namespace F00F;

using static RigidBody3D;

public static class VehicleConfigExtensions
{
    public static void Init<T>(this VehicleConfig cfg, T vehicle, Action InitDrive, Action InitSteering) where T : IVehicle
    {
        if (cfg is null) return;

        cfg.Init(vehicle);
        cfg.DriveTypeSet += InitDrive;
        cfg.SteerTypeSet += InitSteering;
        cfg.Update(InitDrive, InitSteering);

        if (vehicle is RigidBody3D body)
        {
            body.CenterOfMassMode = CenterOfMassModeEnum.Custom;
            body.CenterOfMass = vehicle.WheelCenter();
        }
    }

    public static void Update(this VehicleConfig cfg, Action InitDrive, Action InitSteering)
    {
        if (cfg is null) return;
        InitDrive(); InitSteering();
    }

    public static float GetSteerAngle(this VehicleConfig cfg, Node3D wheel, float steering)
        => cfg.IsSteerWheel(wheel) ? (wheel.IsBack() ? -steering : steering) : 0;

    //public static bool IsDrive(this VehicleConfig cfg, WheelAction type) => type switch
    //{
    //    WheelAction.All => cfg.DriveType is WheelAction.All,
    //    WheelAction.Back => cfg.DriveType is WheelAction.All or WheelAction.Back,
    //    WheelAction.Front => cfg.DriveType is WheelAction.All or WheelAction.Front,
    //    _ => throw new NotImplementedException(),
    //};

    //public static bool IsSteer(this VehicleConfig cfg, WheelAction type) => type switch
    //{
    //    WheelAction.All => cfg.SteerType is WheelAction.All,
    //    WheelAction.Back => cfg.SteerType is WheelAction.All or WheelAction.Back,
    //    WheelAction.Front => cfg.SteerType is WheelAction.All or WheelAction.Front,
    //    _ => throw new NotImplementedException(),
    //};

    public static bool IsDriveWheel(this VehicleConfig cfg, Node3D wheel) => Is(cfg.DriveType, wheel);
    public static bool IsSteerWheel(this VehicleConfig cfg, Node3D wheel) => Is(cfg.SteerType, wheel);

    private static bool Is(WheelAction type, Node3D wheel) => type switch
    {
        WheelAction.All => true,
        WheelAction.Back => wheel.IsBack(),
        WheelAction.Front => wheel.IsFront(),
        _ => throw new NotImplementedException(),
    };
}
