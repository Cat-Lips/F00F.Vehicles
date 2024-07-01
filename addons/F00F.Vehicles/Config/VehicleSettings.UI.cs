using System;
using System.Collections.Generic;
using ControlPair = (Godot.Control Label, Godot.Control EditControl);

namespace F00F.Vehicles
{
    public partial class VehicleSettings : IEditable<VehicleSettings>
    {
        public IEnumerable<ControlPair> GetEditControls() => GetEditControls(out var _);
        public IEnumerable<ControlPair> GetEditControls(out Action<VehicleSettings> SetData)
        {
            var ec = EditControls(out SetData);
            SetData(this);
            return ec;
        }

        public static IEnumerable<ControlPair> EditControls(out Action<VehicleSettings> SetData)
        {
            return UI.Create(out SetData, CreateUI);

            static void CreateUI(UI.IBuilder ui)
            {
                ui.AddGroup("Physics");
                ui.AddValue(nameof(Turbo));
                ui.AddValue(nameof(Brake));
                ui.AddValue(nameof(Bounce));
                ui.AddValue(nameof(Reverse));
                ui.AddValue(nameof(SteerSpeed));
                ui.AddValue(nameof(Acceleration));
                ui.AddValue(nameof(Deceleration));

                ui.AddGroup("MaxValues");
                ui.AddValue(nameof(MaxSpeed));
                ui.AddValue(nameof(MaxTurbo));
                ui.AddValue(nameof(MaxSteer), range: (0, Const.Deg90, null));
                ui.AddValue(nameof(MaxReverse));
                ui.EndGroup();
                ui.EndGroup();

                ui.AddGroup("Features");
                ui.AddOption(nameof(DriveType), items: UI.Items<WheelAction>());
                ui.AddOption(nameof(SteerType), items: UI.Items<WheelAction>());
                ui.EndGroup();
            }
        }
    }
}
