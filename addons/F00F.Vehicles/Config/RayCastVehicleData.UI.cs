using System;
using System.Collections.Generic;
using System.Linq;
using ControlPair = (Godot.Control Label, Godot.Control EditControl);

namespace F00F.Vehicles
{
    public partial class RayCastVehicleData
    {
        public override IEnumerable<ControlPair> GetEditControls(out Action<VehicleData> SetData)
        {
            var ec = RayCastVehicleData.EditControls(out var SetRayCastData);
            SetData = x => SetRayCastData(x as RayCastVehicleData);
            SetData(this);
            return ec;
        }

        public static IEnumerable<ControlPair> EditControls(out Action<RayCastVehicleData> SetData)
        {
            var vehicleControls = VehicleData.EditControls(out var SetVehicleData);
            var rayCastControls = UI.Create<RayCastVehicleData>(out var SetRayCastData, CreateUI);
            SetData = x => { SetVehicleData(x); SetRayCastData(x); };
            return vehicleControls.Concat(rayCastControls);

            static void CreateUI(UI.IBuilder ui)
            {
            }
        }
    }
}
