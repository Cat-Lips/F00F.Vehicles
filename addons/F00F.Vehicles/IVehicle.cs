using Godot;

namespace F00F;

public partial interface IVehicle : IActive, ITarget
{
    #region Config

    Node3D Node => (Node3D)this;
    PhysicsBody3D Body => (PhysicsBody3D)this;
    CollisionObject3D Collider => (CollisionObject3D)this;

    VehicleInput Input { get; set; }
    [Export] VehicleModel Model { get; set; }
    [Export] VehicleConfig Config { get; set; }
    [Export] WheelConfig WheelConfig { get; set; }

    #endregion

    StringName Name { get; }

    //float Mass { get; }
    //float Speed { get; }
    //float Grip { get; }
    //bool Grounded { get; }

    //Vector3 Velocity { get; }
    //Vector3 Direction { get; }
}
