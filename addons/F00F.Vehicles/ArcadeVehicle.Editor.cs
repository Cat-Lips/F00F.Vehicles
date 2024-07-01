#if TOOLS
using Godot.Collections;

namespace F00F.Vehicles
{
    public partial class ArcadeVehicle
    {
        public override void _ValidateProperty(Dictionary property)
        {
            if (this.IsSceneRoot())
            {
                if (Editor.SetDisplayOnly(property, PropertyName.Config)) return;
                if (Editor.SetDisplayOnly(property, PropertyName.AngularDamp)) return;
                if (Editor.SetDisplayOnly(property, PropertyName.PhysicsMaterialOverride)) return;
            }
        }
    }
}
#endif
