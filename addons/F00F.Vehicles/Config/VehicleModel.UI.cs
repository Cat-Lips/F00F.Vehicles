using System;
using System.Collections.Generic;
using ControlPair = (Godot.Control Label, Godot.Control EditControl);

namespace F00F.Vehicles
{
    public partial class VehicleModel : IEditable<VehicleModel>
    {
        public IEnumerable<ControlPair> GetEditControls() => GetEditControls(out var _);
        public IEnumerable<ControlPair> GetEditControls(out Action<VehicleModel> SetData)
        {
            var ec = EditControls(out SetData);
            SetData(this);
            return ec;
        }

        public static IEnumerable<ControlPair> EditControls(out Action<VehicleModel> SetData)
        {
            return UI.Create(out SetData, CreateUI);

            static void CreateUI(UI.IBuilder ui)
            {
                ui.AddScene(nameof(Model));
                ui.AddCheck(nameof(Flip));
                ui.AddValue(nameof(Mass), range: (0, null, null));
                ui.AddValue(nameof(Scale), range: (0, null, null));
            }
        }
    }
}
