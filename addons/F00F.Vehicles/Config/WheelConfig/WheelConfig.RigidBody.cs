using System.Collections.Generic;
using Godot;

namespace F00F;

public partial class WheelConfig
{
    public void Init(RigidBodyVehicle vehicle, IEnumerable<RigidBody3D> wheels)
        => Init(vehicle);
}
