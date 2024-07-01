using System;
using System.Collections.Generic;
using ControlPair = (Godot.Control Label, Godot.Control EditControl);

namespace F00F.Vehicles
{
    public partial class VehicleData : IEditable<VehicleData>
    {
        public virtual IEnumerable<ControlPair> GetEditControls() => GetEditControls(out var _);
        public abstract IEnumerable<ControlPair> GetEditControls(out Action<VehicleData> SetData);

        protected static IEnumerable<ControlPair> EditControls(out Action<VehicleData> SetData)
        {
            var modelControls = VehicleModel.EditControls(out var SetModelData);
            var settingsControls = VehicleModel.EditControls(out var SetSettingsData);

            return UI.Create(out SetData, CreateUI);

            void CreateUI(UI.IBuilder ui)
            {
                ui.AddResource(nameof(Model), nullable: false, controls: modelControls, SetData: SetModelData);
                ui.AddResource(nameof(Settings), nullable: false, controls: settingsControls, SetData: SetSettingsData);
            }
        }
    }
}
