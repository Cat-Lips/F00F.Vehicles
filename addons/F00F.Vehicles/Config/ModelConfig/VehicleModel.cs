using Godot;

namespace F00F;

[Tool, GlobalClass]
public partial class VehicleModel : PhysicsConfig
{
    #region Defaults

    public static class Default
    {
        public const float MassMultiplier = 100;

        public static readonly GlbShapeType[] AllowedBodyShapes = [GlbShapeType.None, GlbShapeType.Convex, GlbShapeType.Box, GlbShapeType.Sphere, GlbShapeType.Capsule];
        public static readonly GlbShapeType[] AllowedPartShapes = [GlbShapeType.None, GlbShapeType.MultiConvex];
        public static readonly GlbShapeType[] AllowedWheelShapes = [GlbShapeType.None, GlbShapeType.Convex, GlbShapeType.CylinderX, GlbShapeType.RayCastY, GlbShapeType.RayShapeY, GlbShapeType.Wheel];
    }

    #endregion

    #region Export

    [ExportGroup("ShapeTypes")]
    [Export] public GlbShapeType BodyShape { get; set => this.Set(ref field, value); }
    [Export] public GlbShapeType PartsShape { get; set => this.Set(ref field, value); }
    [Export] public GlbShapeType WheelShape { get; set => this.Set(ref field, value); }

    #endregion

    #region Private

    internal GlbShapeType[] AllowedBodyShapes { get; private set; } = Default.AllowedBodyShapes;
    internal GlbShapeType[] AllowedPartShapes { get; private set; } = Default.AllowedPartShapes;
    internal GlbShapeType[] AllowedWheelShapes { get; private set; } = Default.AllowedWheelShapes;

    internal void SetAllowedShapes(GlbShapeType[] bodyShapes, GlbShapeType[] partShapes, GlbShapeType[] wheelShapes)
    {
        AllowedBodyShapes = bodyShapes ?? Default.AllowedBodyShapes;
        AllowedPartShapes = partShapes ?? Default.AllowedPartShapes;
        AllowedWheelShapes = wheelShapes ?? Default.AllowedWheelShapes;
        NotifyPropertyListChanged();
    }

    internal void SetDefaultShapes(GlbShapeType bodyShape, GlbShapeType partsShape, GlbShapeType wheelShape)
    {
        DisableChangedEvent();
        BodyShape = bodyShape;
        PartsShape = partsShape;
        WheelShape = wheelShape;
        EnableChangedEvent();
    }

    public VehicleModel()
        => MassMultiplier = Default.MassMultiplier;

    #endregion
}
