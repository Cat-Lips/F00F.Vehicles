using System;
using System.Collections.Generic;
using System.Linq;
using ControlPair = (Godot.Control Label, Godot.Control EditControl);

namespace F00F.Vehicles
{
    public partial class GodotVehicleData
    {
        public override IEnumerable<ControlPair> GetEditControls(out Action<VehicleData> SetData)
        {
            var ec = GodotVehicleData.EditControls(out var SetGodotData);
            SetData = x => SetGodotData(x as GodotVehicleData);
            SetGodotData(this);
            return ec;
        }

        public static IEnumerable<ControlPair> EditControls(out Action<GodotVehicleData> SetData)
        {
            var vehicleControls = VehicleData.EditControls(out var SetVehicleData);
            var godotControls = UI.Create<GodotVehicleData>(out var SetGodotData, CreateUI);
            SetData = x => { SetVehicleData(x); SetGodotData(x); };
            return vehicleControls.Concat(godotControls);

            static void CreateUI(UI.IBuilder ui)
            {
                ui.AddGroup("Wheels", "Wheel");
                ui.AddValue(nameof(WheelRollInfluence), ToolTips.WheelRollInfluence, range: (0, 1, .01f));
                ui.AddValue(nameof(WheelRestLength), ToolTips.WheelRestLength, range: (0, 1, .01f));
                ui.AddValue(nameof(WheelFrictionSlip), ToolTips.WheelFrictionSlip, range: (0, 1, .01f));
                ui.EndGroup();
                ui.AddGroup("Suspension", "Suspension");
                ui.AddValue(nameof(SuspensionTop), ToolTips.SuspensionTop, range: (0, .5f, .01f));
                ui.AddValue(nameof(SuspensionTravel), ToolTips.SuspensionTravel, range: (0, .5f, .01f));
                ui.AddValue(nameof(SuspensionStiffness), ToolTips.SuspensionStiffness, range: (0, null, 10)); // 50-200
                ui.AddValue(nameof(SuspensionForceMultiplier), ToolTips.SuspensionForceMultiplier, range: (0, null, 1000)); // eg, 10k
                ui.EndGroup();
                ui.AddGroup("Damping", "Damping");
                ui.AddValue(nameof(DampingCompression), ToolTips.DampingCompression, range: (0, 1, .01f));
                ui.AddValue(nameof(DampingRelaxation), ToolTips.DampingRelaxation, range: (0, 1, .01f));
                ui.EndGroup();
            }
        }

        private static class ToolTips
        {
            public static readonly string WheelRollInfluence = @"
This value affects the roll of your vehicle. If set to 1.0 for all wheels, your
vehicle will resist body roll, while a value of 0.0 will be prone to rolling
over.".TrimStart();
            public static readonly string WheelRestLength = @"
This is the distance in meters the wheel is lowered from its origin point. Don't
set this to 0.0 and move the wheel into position, instead move the origin point
of your wheel (the gizmo in Godot) to the position the wheel will take when bottoming
out, then use the rest length to move the wheel down to the position it should
be in when the car is in rest.".TrimStart();
            public static readonly string WheelFrictionSlip = @"
This determines how much grip this wheel has. It is combined with the friction
setting of the surface the wheel is in contact with. 0.0 means no grip, 1.0 is
normal grip. For a drift car setup, try setting the grip of the rear wheels slightly
lower than the front wheels, or use a lower value to simulate tire wear.

It's best to set this to 1.0 when starting out.".TrimStart();
            public static readonly string SuspensionTop = @"
This is the distance above the wheel the suspension can travel.
The wheel position will be adjusted to this value.".TrimStart();
            public static readonly string SuspensionTravel = @"
This is the distance the suspension can travel. As Godot units are equivalent
to meters, keep this setting relatively low. Try a value between 0.1 and 0.3
depending on the type of car.".TrimStart();
            public static readonly string SuspensionStiffness = @"
This value defines the stiffness of the suspension. Use a value lower than 50
for an off-road car, a value between 50 and 100 for a race car and try something
around 200 for something like a Formula 1 car.".TrimStart();
            public static readonly string SuspensionMaxForce = @"
The maximum force the spring can resist. This value should be higher than a quarter
of the Mass of the VehicleBody or the spring will not
carry the weight of the vehicle. Good results are often obtained by a value that
is about 3× to 4× this number.".TrimStart();
            public static readonly string SuspensionForceMultiplier = $@"
The multiplier used to calculate SuspensionMaxForce (defined as '{SuspensionMaxForce}')".TrimStart();
            public static readonly string DampingCompression = @"
The damping applied to the spring when the spring is being compressed. This value
should be between 0.0 (no damping) and 1.0. A value of 0.0 means the car will
keep bouncing as the spring keeps its energy. A good value for this is around
0.3 for a normal car, 0.5 for a race car.".TrimStart();
            public static readonly string DampingRelaxation = @"
The damping applied to the spring when relaxing. This value should be between
0.0 (no damping) and 1.0. This value should always be slightly higher than the
DampingCompression property. For a DampingCompression
value of 0.3, try a relaxation value of 0.5.".TrimStart();
        }
    }
}
