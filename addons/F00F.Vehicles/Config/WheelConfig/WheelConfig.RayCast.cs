using System.Collections.Generic;
using Godot;

namespace F00F;

public partial class WheelConfig
{
    #region Defaults

    private static class AV_Default
    {
        public const float SpringStrength = 100f;
        public const float SpringDamping = 2f;
        public const float RestDistance = .5f;
    }

    #endregion

    #region Export

    [Export] public float AV_SpringStrength { get; set; } = AV_Default.SpringStrength;
    [Export] public float AV_SpringDamping { get; set; } = AV_Default.SpringDamping;
    [Export] public float AV_RestDistance { get; set; } = AV_Default.RestDistance;

    #endregion

    public void Init(RayCastVehicle vehicle, IEnumerable<RayCast3D> wheels)
        => Init(vehicle);

    public void ApplySuspension(RayCastVehicle vehicle, ICollection<RayCast3D> wheels)
    {
        wheels.ForEach(x =>
        {
            if (x.IsColliding())
            {
                var springTransform = x.GlobalTransform;
                var springPosition = springTransform.Origin;
                var springLoad = vehicle.Mass / wheels.Count;
                var springUp = springTransform.Up();

                var ground = x.GetCollisionPoint();
                var length = springPosition.DistanceTo(ground);
                var offset = AV_RestDistance - length;

                var springForce = offset * AV_SpringStrength * springLoad;
                var springVelocity = springUp.Dot(vehicle.GetPointVelocity(ground));
                var springDampForce = springVelocity * AV_SpringDamping * springLoad;

                var forceVector = (springForce - springDampForce) * springUp;
                var forcePosition = ground - vehicle.GlobalPosition;

                vehicle.ApplyForce(forceVector, forcePosition);
                vehicle._DrawForce(forceVector, forcePosition);
            }
        });
    }
}
