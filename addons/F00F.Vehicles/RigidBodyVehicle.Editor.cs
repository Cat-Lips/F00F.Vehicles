﻿#if TOOLS
using Godot.Collections;

namespace F00F.Vehicles
{
    public partial class RigidBodyVehicle
    {
        public override void _ValidateProperty(Dictionary property)
        {
            if (this.IsSceneRoot())
            {
                if (Editor.SetDisplayOnly(property, PropertyName.Config)) return;
            }
        }
    }
}
#endif
