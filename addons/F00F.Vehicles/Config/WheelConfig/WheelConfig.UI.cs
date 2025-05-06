using System;
using System.Collections.Generic;
using Godot;

namespace F00F;

using ControlPair = (Control Label, Control EditControl);

public partial class WheelConfig : IEditable<WheelConfig>
{
    public IEnumerable<ControlPair> GetEditControls() => GetEditControls(out var _);
    public IEnumerable<ControlPair> GetEditControls(out Action<WheelConfig> SetData)
    {
        var ec = EditControls(out SetData, VehicleType);
        SetData(this);
        return ec;
    }

    public static IEnumerable<ControlPair> EditControls(out Action<WheelConfig> SetData, VehicleType type)
    {
        return UI.Create(out SetData, CreateUI);

        void CreateUI(UI.IBuilder ui)
        {
            switch (type)
            {
                case VehicleType.Godot: GDV_UI(ui); break;
                case VehicleType.Arcade: AV_UI(ui); break;
                case VehicleType.RayCast: RCV_UI(ui); break;
                case VehicleType.RigidBody: RBV_UI(ui); break;
                case VehicleType.CharacterBody: CBV_UI(ui); break;
                default: throw new NotImplementedException();
            }
        }
    }
}
