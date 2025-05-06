using System.Collections.Generic;
using Godot;

namespace F00F;

public partial class WheelConfig
{
    public void Init(ArcadeVehicle root, IEnumerable<Node3D> wheels)
        => Init(root);
}
