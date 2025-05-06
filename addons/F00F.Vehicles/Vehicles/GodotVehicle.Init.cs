using System.Collections.Generic;
using System.Linq;
using Godot;

namespace F00F;

public partial class GodotVehicle
{
    private static class Default
    {
        public const GlbShapeType BodyShape = GlbShapeType.Convex;
        public const GlbShapeType PartsShape = GlbShapeType.None;
        public const GlbShapeType WheelShape = GlbShapeType.Wheel;

        public static readonly GlbShapeType[] AllowedBodyShapes = [GlbShapeType.Convex, GlbShapeType.Capsule, GlbShapeType.Box];
        public static readonly GlbShapeType[] AllowedPartShapes = [GlbShapeType.None, GlbShapeType.MultiConvex];
        public static readonly GlbShapeType[] AllowedWheelShapes = [GlbShapeType.Wheel];
    }

    private readonly List<VehicleWheel3D> wheels = [];
    private VehicleWheel3D[] rearWheels, frontWheels, driveWheels, steerWheels;

    private void OnModelSet()
    {
        Model.Init(Default.BodyShape, Default.PartsShape, Default.WheelShape);
        Model.Init(Default.AllowedBodyShapes, Default.AllowedPartShapes, Default.AllowedWheelShapes);
        Model.Init(this, OnPreInit, OnPostInit, OnPartAdded);

        void OnPreInit()
            => wheels.Clear();

        void OnPostInit()
        {
            rearWheels = wheels.Where(x => x.IsBack()).ToArray();
            frontWheels = wheels.Where(x => x.IsFront()).ToArray();

            Config.Update(InitDrive, InitSteering);

            InitWheels();
        }

        void OnPartAdded(Node3D part)
        {
            if (Model.IsWheel(part) && part is VehicleWheel3D wheel)
                wheels.Add(wheel);
        }
    }

    private void OnConfigSet()
        => Config?.Init(this, InitDrive, InitSteering);

    private void InitDrive()
        => driveWheels = [.. wheels.Where(x => x.UseAsTraction = Config.IsDriveWheel(x))];

    private void InitSteering()
        => steerWheels = [.. wheels.Where(x => x.UseAsSteering = Config.IsSteerWheel(x))];

    private void InitWheels()
        => WheelConfig?.Init(this, wheels);
}
