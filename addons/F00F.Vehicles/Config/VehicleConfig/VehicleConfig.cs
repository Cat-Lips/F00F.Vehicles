using System;
using Godot;

namespace F00F;

public enum WheelAction
{
    All,
    Back,
    Front,
}

[Tool, GlobalClass]
public partial class VehicleConfig : BaseConfig
{
    public event Action DriveTypeSet;
    public event Action SteerTypeSet;
    public event Action StabilitySet;

    [ExportGroup("Steering")]
    [Export] public WheelAction SteerType { get; set => this.Set(ref field, value, SteerTypeSet); } = WheelAction.Front;
    [Export(PropertyHint.Range, "0,90,radians_as_degrees")] public float MaxSteerAngle { get; set; } = Const.Deg30;
    [Export(PropertyHint.Range, "0,10,or_greater")] public float MaxSteerSpeed { get; set; } = 3;

    [ExportGroup("Driving")]
    [Export] public WheelAction DriveType { get; set => this.Set(ref field, value, DriveTypeSet); } = WheelAction.Back;
    [Export(PropertyHint.Range, "-1,1,or_greater,or_less")] public float StabilityY { get; set => this.Set(ref field, value, StabilitySet); } // Relative to wheel center as a ratio of wheel placement (ie, 0 = wheel center, 1/-1 = higher/lower on vehicle)
    [Export(PropertyHint.Range, "-1,1,or_greater,or_less")] public float StabilityZ { get; set => this.Set(ref field, value, StabilitySet); } // Relative to wheel center as a ratio of wheel placement (ie, 0 = wheel center, 1/-1 = front/rear axle)

    [ExportGroup("Features")]
    [Export(PropertyHint.Range, "0,10,or_greater,or_less")] public float JumpStrength { get; set; } = 5;

    [ExportGroup("GodotVehicle", "GDV_")]
    [Export] public int GDV_MaxRPM { get; set; } = 450;
    [Export] public int GDV_MaxTorque { get; set; } = 300;
    [Export] public float GDV_BrakeForce { get; set; } = .1f;

    [ExportGroup("ArcadeVehicle", "AV_")]
    [Export] public int AV_1 { get; set; }
    [Export] public int AV_2 { get; set; }

    [ExportGroup("RayCastVehicle", "RCV_")]
    [Export] public int RCV_1 { get; set; }
    [Export] public int RCV_2 { get; set; }

    [ExportGroup("RigidBodyVehicle", "RBV_")]
    [Export] public int RBV_1 { get; set; }
    [Export] public int RBV_2 { get; set; }

    [ExportGroup("CharacterBodyVehicle", "CBV_")]
    [Export] public int CBV_1 { get; set; }
    [Export] public int CBV_2 { get; set; }
}
