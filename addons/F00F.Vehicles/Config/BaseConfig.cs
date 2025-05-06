using System;
using Godot;

namespace F00F;

public enum VehicleType
{
    Godot,
    Arcade,
    RayCast,
    RigidBody,
    CharacterBody
}

public partial interface IVehicle
{
    static IVehicle New(VehicleType type) => type switch
    {
        VehicleType.Godot => Utils.New<GodotVehicle>(),
        VehicleType.Arcade => Utils.New<ArcadeVehicle>(),
        VehicleType.RayCast => Utils.New<RayCastVehicle>(),
        VehicleType.RigidBody => Utils.New<RigidBodyVehicle>(),
        VehicleType.CharacterBody => Utils.New<CharacterBodyVehicle>(),
        _ => throw new NotImplementedException(),
    };

    static string Tag(VehicleType type) => type switch
    {
        VehicleType.Godot => "GDV",
        VehicleType.Arcade => "AV",
        VehicleType.RayCast => "RCV",
        VehicleType.RigidBody => "RBV",
        VehicleType.CharacterBody => "CBV",
        _ => throw new NotImplementedException(),
    };
}

[Tool]
public abstract partial class BaseConfig : CustomResource
{
    protected VehicleType VehicleType { get; set => this.Set(ref field, value, notify: true); }

    internal void Init(IVehicle vehicle)
    {
        VehicleType = vehicle.GetType() switch
        {
            var t when t == typeof(GodotVehicle) => VehicleType.Godot,
            var t when t == typeof(ArcadeVehicle) => VehicleType.Arcade,
            var t when t == typeof(RayCastVehicle) => VehicleType.RayCast,
            var t when t == typeof(RigidBodyVehicle) => VehicleType.RigidBody,
            var t when t == typeof(CharacterBodyVehicle) => VehicleType.CharacterBody,
            _ => throw new NotImplementedException(),
        };
    }
}
