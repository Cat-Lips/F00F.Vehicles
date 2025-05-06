using System;
using System.Collections.Generic;
using Godot;

namespace F00F;

public partial class WheelConfig
{
    #region Defaults

    private static class RCV_Default
    {
        public const float SpringStrength = 100f;
        public const float SpringDamping = 1f;
        public const float SpringLength = 0f;
    }

    #endregion

    #region Actions

    private Action RCV_SpringLengthSet;

    #endregion

    #region Export

    [Export(PropertyHint.Range, "0,100")] public float RCV_SpringStrength { get; set; } = RCV_Default.SpringStrength;
    [Export(PropertyHint.Range, "0,1")] public float RCV_SpringDamping { get; set; } = RCV_Default.SpringDamping;
    [Export(PropertyHint.Range, "0,1")] public float RCV_SpringLength { get; set => this.Set(ref field, value, RCV_SpringLengthSet); } = RCV_Default.SpringLength;

    #endregion

    public record RCV_Wheel
    {
        public RayCast3D Suspension { get; }
        public MeshInstance3D Wheel { get; }
        public float Radius { get; }
        public float RestDistance(float springLength) => (1 + springLength) * Radius;

        public RCV_Wheel(RayCast3D part)
        {
            Suspension = part ?? throw new ArgumentNullException(nameof(part));
            Wheel = part.GetNode<MeshInstance3D>("Mesh");
            Radius = Wheel.GetAabb().Size.Y * .5f;
        }
    }

    public void Init(RayCastVehicle vehicle, IEnumerable<RCV_Wheel> wheels)
    {
        Init(vehicle);
        InitWheels();
        SetActions();

        void InitWheels()
            => SetSpringLength();

        void SetActions()
            => RCV_SpringLengthSet = SetSpringLength;

        void SetSpringLength()
            => wheels.ForEach(x => x.Suspension.TargetPosition = x.RestDistance(RCV_SpringLength) * Vector3.Down);
    }

    public void ApplySuspension(RayCastVehicle vehicle, ICollection<RCV_Wheel> wheels)
    {
        wheels.ForEach(x =>
        {
            if (x.Suspension.IsColliding())
            {
                var springTransform = x.Suspension.GlobalTransform;
                var springPosition = springTransform.Origin;
                var springLoad = vehicle.Mass / wheels.Count;
                var springUp = springTransform.Up();

                var ground = x.Suspension.GetCollisionPoint();
                var length = springPosition.DistanceTo(ground) - x.Radius;
                var offset = x.RestDistance(RCV_SpringLength) - length;

                x.Wheel.Position = x.Wheel.Position.With(y: -length);

                var springForce = RCV_SpringStrength * springLoad * offset;
                var springVelocity = springUp.Dot(vehicle.GetPointVelocity(ground));
                var springDampForce = RCV_SpringDamping * RCV_SpringStrength * springLoad * springVelocity;

                var forceVector = (springForce - springDampForce) * springUp;
                var forcePosition = ground - vehicle.GlobalPosition;

                vehicle.ApplyForce(forceVector, forcePosition);
                vehicle._DrawForce(forceVector, forcePosition);
            }
        });
    }
}
