using System;
using Godot;

namespace F00F;

[Tool]
public partial class CharacterBodyVehicle : RigidBody3D, IVehicle
{
    public event Action ActiveSet;
    public bool Active { get; set => this.Set(ref field, value, ActiveSet); }

    #region Config

    public VehicleInput Input { get; set => this.Set(ref field, value ?? new()); }
    [Export] public VehicleModel Model { get; set => this.Set(ref field, value ?? new(), OnModelSet); }
    [Export] public VehicleConfig Config { get; set => this.Set(ref field, value ?? new(), OnConfigSet); }
    [Export] public WheelConfig WheelConfig { get; set => this.Set(ref field, value ?? new(), InitWheels); }

    #endregion

    //public float Mass { get; private set; }
    public float Speed { get; private set; }
    public float Grip { get; private set; }
    public bool Grounded { get; private set; }

    public Vector3 Velocity { get; private set; }
    public Vector3 Direction { get; private set; }

    #region Godot

    public sealed override void _Ready()
    {
        Input ??= new();
        Model ??= new();
        Config ??= new();
        WheelConfig ??= new();
        this.InitCollisions();
        Editor.Disable(this);
    }

    #endregion
}
