using System.Collections.Generic;
using System.Linq;
using Godot;

namespace F00F;

public partial class RayCastVehicle
{
    private static class Default
    {
        public const GlbShapeType BodyShape = GlbShapeType.Convex;
        public const GlbShapeType PartsShape = GlbShapeType.None;
        public const GlbShapeType WheelShape = GlbShapeType.RayCastY;

        public static readonly GlbShapeType[] AllowedBodyShapes = [GlbShapeType.None, GlbShapeType.Convex, GlbShapeType.Capsule, GlbShapeType.Box];
        public static readonly GlbShapeType[] AllowedPartShapes = [GlbShapeType.None, GlbShapeType.MultiConvex];
        public static readonly GlbShapeType[] AllowedWheelShapes = [GlbShapeType.None, GlbShapeType.RayCastY];
    }

    private readonly List<RayCast3D> wheels = [];
    private RayCast3D[] rearWheels, frontWheels, driveWheels, steerWheels;

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
            if (Model.IsWheel(part) && part is RayCast3D ray)
                wheels.Add(ray);
        }
    }

    private void OnConfigSet()
        => Config.Init(this, InitDrive, InitSteering);

    private void InitDrive()
        => driveWheels = wheels.Where(Config.IsDriveWheel).ToArray();

    private void InitSteering()
        => steerWheels = wheels.Where(Config.IsSteerWheel).ToArray();

    private void InitWheels()
        => WheelConfig?.Init(this, wheels);
}
