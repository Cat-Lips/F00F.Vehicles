using System.Collections.Generic;
using System.Linq;
using Godot;

namespace F00F;

using static WheelConfig;

public partial class RayCastVehicle
{
    private static class Default
    {
        public const GlbShapeType BodyShape = GlbShapeType.Convex;
        public const GlbShapeType PartsShape = GlbShapeType.None;
        public const GlbShapeType WheelShape = GlbShapeType.RayCastY;

        public static readonly GlbShapeType[] AllowedBodyShapes = [GlbShapeType.Convex, GlbShapeType.Capsule, GlbShapeType.Box];
        public static readonly GlbShapeType[] AllowedPartShapes = [GlbShapeType.None, GlbShapeType.MultiConvex];
        public static readonly GlbShapeType[] AllowedWheelShapes = [GlbShapeType.RayCastY];
    }

    private readonly List<RCV_Wheel> wheels = [];
    private RCV_Wheel[] rearWheels, frontWheels, driveWheels, steerWheels;

    private void OnModelSet()
    {
        Model.Init(Default.BodyShape, Default.PartsShape, Default.WheelShape);
        Model.Init(Default.AllowedBodyShapes, Default.AllowedPartShapes, Default.AllowedWheelShapes);
        Model.Init(this, OnPreInit, OnPostInit, OnPartAdded);

        void OnPreInit()
            => wheels.Clear();

        void OnPostInit()
        {
            rearWheels = wheels.Where(x => x.Wheel.IsBack()).ToArray();
            frontWheels = wheels.Where(x => x.Wheel.IsFront()).ToArray();

            Config.Update(InitDrive, InitSteering);

            InitWheels();
        }

        void OnPartAdded(Node3D part)
        {
            if (Model.IsWheel(part) && part is RayCast3D ray)
                wheels.Add(new(ray));
        }
    }

    private void OnConfigSet()
        => Config.Init(this, InitDrive, InitSteering);

    private void InitDrive()
        => driveWheels = wheels.Where(x => Config.IsDriveWheel(x.Wheel)).ToArray();

    private void InitSteering()
        => steerWheels = wheels.Where(x => Config.IsSteerWheel(x.Wheel)).ToArray();

    private void InitWheels()
        => WheelConfig?.Init(this, wheels);
}
