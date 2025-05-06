#if TOOLS
using Godot.Collections;

namespace F00F;

public partial class WheelConfig
{
    protected sealed override bool ValidateGodotProperties(Dictionary property)
    {
        return
            Editor.SetReadOnly(property, PropertyName.GDV_SuspensionMaxForce)
                && Editor.SetDisplayOnly(property, PropertyName.GDV_SuspensionMaxForce) ||
            Editor.SetReadOnly(property, PropertyName.GDV_SuspensionStiffness)
                && Editor.SetDisplayOnly(property, PropertyName.GDV_SuspensionStiffness);
    }
}
#endif
