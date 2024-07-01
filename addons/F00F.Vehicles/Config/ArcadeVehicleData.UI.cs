using System;
using System.Collections.Generic;
using System.Linq;
using ControlPair = (Godot.Control Label, Godot.Control EditControl);

namespace F00F.Vehicles
{
    public partial class ArcadeVehicleData
    {
        public override IEnumerable<ControlPair> GetEditControls(out Action<VehicleData> SetData)
        {
            var ec = ArcadeVehicleData.EditControls(out var SetArcadeData);
            SetData = x => SetArcadeData(x as ArcadeVehicleData);
            SetArcadeData(this);
            return ec;
        }

        public static IEnumerable<ControlPair> EditControls(out Action<ArcadeVehicleData> SetData)
        {
            var vehicleControls = VehicleData.EditControls(out var SetVehicleData);
            var arcadeControls = UI.Create<ArcadeVehicleData>(out var SetArcadeData, CreateUI);
            SetData = x => { SetVehicleData(x); SetArcadeData(x); };
            return vehicleControls.Concat(arcadeControls);

            static void CreateUI(UI.IBuilder ui)
            {
            }
        }
    }
}
