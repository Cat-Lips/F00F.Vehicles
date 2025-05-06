using System.Collections.Generic;
using Godot;

namespace F00F;

public partial class WheelConfig
{
    public void Init(RigidBodyVehicle root, IEnumerable<RigidBody3D> wheels)
        => Init(root);
}
