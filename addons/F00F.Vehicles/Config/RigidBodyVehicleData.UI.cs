using System;
using System.Collections.Generic;
using System.Linq;
using ControlPair = (Godot.Control Label, Godot.Control EditControl);

namespace F00F.Vehicles
{
    public partial class RigidBodyVehicleData
    {
        public override IEnumerable<ControlPair> GetEditControls(out Action<VehicleData> SetData)
        {
            var ec = RigidBodyVehicleData.EditControls(out var SetRigidBodyData);
            SetData = x => SetRigidBodyData(x as RigidBodyVehicleData);
            SetRigidBodyData(this);
            return ec;
        }

        public static IEnumerable<ControlPair> EditControls(out Action<RigidBodyVehicleData> SetData)
        {
            var vehicleControls = VehicleData.EditControls(out var SetVehicleData);
            var rigidBodyControls = UI.Create<RigidBodyVehicleData>(out var SetRigidBodyData, CreateUI);
            SetData = x => { SetVehicleData(x); SetRigidBodyData(x); };
            return vehicleControls.Concat(rigidBodyControls);

            static void CreateUI(UI.IBuilder ui)
            {
            }
        }
    }
}
