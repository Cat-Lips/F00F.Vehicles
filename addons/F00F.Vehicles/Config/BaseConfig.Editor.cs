#if TOOLS
using Godot.Collections;

namespace F00F;

public partial class BaseConfig
{
    protected virtual bool ValidateGodotProperties(Dictionary property) => false;
    protected virtual bool ValidateArcadeProperties(Dictionary property) => false;
    protected virtual bool ValidateRayCastProperties(Dictionary property) => false;
    protected virtual bool ValidateRigidBodyProperties(Dictionary property) => false;
    protected virtual bool ValidateCharacterBodyProperties(Dictionary property) => false;

    public sealed override void _ValidateProperty(Dictionary property)
    {
        ShowRequiredProperties();

        if (ValidateGodotProperties(property)) return;
        if (ValidateArcadeProperties(property)) return;
        if (ValidateRayCastProperties(property)) return;
        if (ValidateRigidBodyProperties(property)) return;
        if (ValidateCharacterBodyProperties(property)) return;

        void ShowRequiredProperties()
        {
            var p = (string)property["name"];

            if (p.Contains('_'))
                Editor.Show(property, p, @if: p.StartsWith($"{IVehicle.Tag(VehicleType)}_"));
        }
    }
}
#endif
