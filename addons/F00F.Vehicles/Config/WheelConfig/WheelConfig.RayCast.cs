using System.Collections.Generic;
using Godot;

namespace F00F;

public partial class WheelConfig
{
    public void Init(RayCastVehicle vehicle, IEnumerable<RayCast3D> wheels)
        => Init(vehicle);
}
