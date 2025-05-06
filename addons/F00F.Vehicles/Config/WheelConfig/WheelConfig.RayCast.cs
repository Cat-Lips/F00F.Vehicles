using System.Collections.Generic;
using Godot;

namespace F00F;

public partial class WheelConfig
{
    public void Init(RayCastVehicle root, IEnumerable<RayCast3D> wheels)
        => Init(root);
}
