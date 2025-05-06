using System;
using Godot;

namespace F00F;

public static class VehicleModelExtensions
{
    private static readonly StringName BodyTag = "_VM_Body_";
    private static readonly StringName PartTag = "_VM_Part_";
    private static readonly StringName WheelTag = "_VM_Wheel_";

    public static void Init(this VehicleModel cfg, GlbShapeType[] bodyShapes = null, GlbShapeType[] partShapes = null, GlbShapeType[] wheelShapes = null)
        => cfg.SetAllowedShapes(bodyShapes, partShapes, wheelShapes);

    public static void Init(this VehicleModel cfg, GlbShapeType bodyShape = GlbShapeType.None, GlbShapeType partsShape = GlbShapeType.None, GlbShapeType wheelShape = GlbShapeType.None)
        => cfg.SetDefaultShapes(bodyShape, partsShape, wheelShape);

    public static void Init(this VehicleModel cfg,
        CollisionObject3D root,
        Action OnPreInit = null,
        Action OnPostInit = null,
        Action<Node3D> OnPartAdded = null)
    {
        var tag = string.Empty;
        cfg.Init(root, OnPreInit, OnPostInit, GetShapeType, null, null, TagAddedPart);

        GlbShapeType GetShapeType(MeshInstance3D mesh)
        {
            if (mesh.HasParent<MeshInstance3D>())
            {
                tag = PartTag;
                return cfg.PartsShape;
            }

            if (mesh.Name.ContainsN("wheel") || (mesh.GetParent()?.Name.ContainsN("wheel") ?? false))
            {
                tag = WheelTag;
                return cfg.WheelShape;
            }

            tag = BodyTag;
            return cfg.BodyShape;
        }

        void TagAddedPart(Node3D part)
        {
            part.SetMeta(tag, true);
            OnPartAdded?.Invoke(part);
        }
    }

    public static bool IsBody(this VehicleModel _, Node3D part) => (bool)part.GetMeta(BodyTag, false);
    public static bool IsPart(this VehicleModel _, Node3D part) => (bool)part.GetMeta(PartTag, false);
    public static bool IsWheel(this VehicleModel _, Node3D part) => (bool)part.GetMeta(WheelTag, false);
}
