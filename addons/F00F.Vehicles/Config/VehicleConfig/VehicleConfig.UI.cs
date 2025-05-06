using System;
using System.Collections.Generic;
using Godot;

namespace F00F;

using ControlPair = (Control Label, Control EditControl);

public partial class VehicleConfig : IEditable<VehicleConfig>
{
    public IEnumerable<ControlPair> GetEditControls() => GetEditControls(out var _);
    public IEnumerable<ControlPair> GetEditControls(out Action<VehicleConfig> SetData)
    {
        var ec = EditControls(out SetData);
        SetData(this);
        return ec;
    }

    public static IEnumerable<ControlPair> EditControls(out Action<VehicleConfig> SetData)
    {
        return UI.Create(out SetData, CreateUI);

        static void CreateUI(UI.IBuilder ui)
        {
            ui.AddOption(nameof(SteerType), items: UI.Items<WheelAction>());
            ui.AddOption(nameof(DriveType), items: UI.Items<WheelAction>());
            ui.AddValue(nameof(JumpStrength), range: (0, null, null));
        }
    }
}
