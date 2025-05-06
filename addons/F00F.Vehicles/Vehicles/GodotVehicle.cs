using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace F00F;

[Tool]
public partial class GodotVehicle : VehicleBody3D, IVehicle
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
    public bool Grounded { get; private set; }
    public Vector3 Velocity { get; private set; }

    public IEnumerable<WheelInfo> GetWheelInfo()
    {
        return wheels.Select(x => new WheelInfo(
            x,
            x.IsInContact(),
            x.GetContactBody(),
            x.GetContactPoint(),
            x.GetContactNormal(),
            x.GetSkidinfo(),
            x.GetRpm()));
    }

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

    private float steering;
    public sealed override void _PhysicsProcess(double _delta)
    {
        var delta = (float)_delta;
        Grounded = this.IsColliding() || wheels.Any(x => x.IsInContact());

        Velocity = LinearVelocity;
        Speed = Velocity.Length();

        UpdateSteer();
        UpdateDrive();
        UpdateBrake();
        UpdateJump();

        void UpdateSteer()
        {
            var steer = Active ? Input.Steer() : 0;
            var target = steer * Config.MaxSteerAngle;
            if (steering == target) return;

            steering = Mathf.MoveToward(steering, target, Config.MaxSteerSpeed * delta);
            steerWheels.ForEach(x => x.Steering = x.IsBack() ? -steering : steering);
        }

        void UpdateDrive()
        {
            var drive = Active ? Input.Drive() : 0;
            if (Input.Turbo1()) drive *= Config.TurboMultiplier;
            if (Input.Turbo2()) drive *= Config.TurboMultiplier;
            EngineForce = drive * Config.GDV_Engine * Mass;
        }

        void UpdateBrake()
        {
            Brake = Active && Input.Brake()
                ? Config.GDV_Brake * Mass * .1f
                : 0;
        }

        void UpdateJump()
        {
            if (Active && Grounded && Input.Jump())
                ApplyCentralImpulse(Vector3.Up * (Config.JumpStrength * Mass));
        }
#if DEBUG
        this.UpdateDebugInfo();
#endif
    }

    #endregion
}
