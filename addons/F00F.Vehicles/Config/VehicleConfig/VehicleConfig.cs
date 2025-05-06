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

    [ExportGroup("Steering")]
    [Export] public WheelAction SteerType { get; set => this.Set(ref field, value, SteerTypeSet); } = WheelAction.Front;
    [Export(PropertyHint.Range, "0,90,radians_as_degrees")] public float MaxSteerAngle { get; set; } = Const.Deg30;
    [Export(PropertyHint.Range, "0,10,or_greater")] public float MaxSteerSpeed { get; set; } = 3f;

    [ExportGroup("Driving")]
    [Export] public WheelAction DriveType { get; set => this.Set(ref field, value, DriveTypeSet); } = WheelAction.Back;

    [ExportGroup("Driving", "GDV_")]
    [Export(PropertyHint.Range, "0,1,or_greater")] public float GDV_Engine { get; set; } = 1f;
    [Export(PropertyHint.Range, "0,1,or_greater")] public float GDV_Brake { get; set; } = .1f;

    [ExportGroup("Driving", "AV_")]
    [Export] public int AV_1 { get; set; }
    [Export] public int AV_2 { get; set; }

    [ExportGroup("Driving", "RCV_")]
    [Export] public int RCV_1 { get; set; }
    [Export] public int RCV_2 { get; set; }

    [ExportGroup("Driving", "RBV_")]
    [Export] public int RBV_1 { get; set; }
    [Export] public int RBV_2 { get; set; }

    [ExportGroup("Driving", "CBV_")]
    [Export] public int CBV_1 { get; set; }
    [Export] public int CBV_2 { get; set; }

    [ExportGroup("")]
    [Export(PropertyHint.Range, "0,1,or_greater")] public float JumpStrength { get; set; } = 1;
}
