#if TOOLS
using Godot.Collections;

namespace F00F;

public partial class VehicleModel
{
    public sealed override void _ValidateProperty(Dictionary property)
    {
        if (Editor.SetEnumHint(property, PropertyName.BodyShape, AllowedBodyShapes)) return;
        if (Editor.SetEnumHint(property, PropertyName.PartsShape, AllowedPartShapes)) return;
        if (Editor.SetEnumHint(property, PropertyName.WheelShape, AllowedWheelShapes)) return;
    }
}
#endif
