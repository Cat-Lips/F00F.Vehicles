namespace F00F;
public partial class WheelConfig : IEditable<WheelConfig>
{
    private static void GDV_UI(UI.IBuilder ui)
    {
        ui.AddGroup("Wheels", "GDV_Wheel");
        ui.AddValue(nameof(GDV_WheelRollInfluence), GDV_ToolTips.WheelRollInfluence, range: (0, 1, .01f));
        ui.AddValue(nameof(GDV_WheelFrictionSlip), GDV_ToolTips.WheelFrictionSlip, range: (0, 1, .01f));
        ui.AddValue(nameof(GDV_WheelRestLength), GDV_ToolTips.WheelRestLength, range: (0, 1, .01f));
        ui.EndGroup();
        ui.AddGroup("Suspension", "GDV_Suspension");
        ui.AddValue(nameof(GDV_SuspensionTravel), GDV_ToolTips.SuspensionTravel, range: (0, .5f, .01f));
        ui.AddValue(nameof(GDV_SuspensionMaxForce), GDV_ToolTips.SuspensionMaxForce, range: (0, null, 1000)); // eg, 10k
        ui.AddValue(nameof(GDV_SuspensionStiffness), GDV_ToolTips.SuspensionStiffness, range: (0, null, 10)); // 50-200
        ui.AddValue(nameof(GDV_SuspensionMaxForceByMass), GDV_ToolTips.SuspensionMaxForceByMass, range: (0, null, .01f));
        ui.AddValue(nameof(GDV_SuspensionStiffnessByMass), GDV_ToolTips.SuspensionStiffnessByMass, range: (0, null, .01f));
        ui.EndGroup();
        ui.AddGroup("Damping", "GDV_Damping");
        ui.AddValue(nameof(GDV_DampingCompression), GDV_ToolTips.DampingCompression, range: (0, 1, .01f));
        ui.AddValue(nameof(GDV_DampingRelaxation), GDV_ToolTips.DampingRelaxation, range: (0, 1, .01f));
        ui.EndGroup();
        ui.AddGroup("Drift", "GDV_Drift");
        ui.AddCheck(nameof(GDV_DriftEnabled), GDV_ToolTips.DriftEnabled);
        ui.AddValue(nameof(GDV_DriftSpread), GDV_ToolTips.DriftSpread, range: (0, 1, null));
        ui.EndGroup();
    }

    private static class GDV_ToolTips
    {
        public static readonly string WheelRollInfluence = @"
This value affects the roll of your vehicle. If set to 1.0 for all wheels, your
vehicle will resist body roll, while a value of 0.0 will be prone to rolling over.".TrimStart();
        public static readonly string WheelRestLength = @"
This is the distance in meters the wheel is lowered from its origin point.
Don't set this to 0.0 and move the wheel into position, instead move the origin
point of your wheel to the position the wheel will take when bottoming out, then
use the rest length to move the wheel down to the position it should be in when
the car is in rest.".TrimStart();
        public static readonly string WheelFrictionSlip = @"
This determines how much grip this wheel has. It is combined with the friction
setting of the surface the wheel is in contact with. 0.0 means no grip, 1.0 is
normal grip. For a drift car setup, try setting the grip of the rear wheels
slightly lower than the front wheels, or use a lower value to simulate tire wear.".TrimStart();

        public static readonly string SuspensionTravel = @"
This is the distance the suspension can travel. As Godot units are equivalent
to meters, keep this setting relatively low. Try a value between 0.1 and 0.3
depending on the type of car.".TrimStart();
        public static readonly string SuspensionStiffness = @"
The stiffness of the suspension, measured in Newtons per millimeter (N/mm), or
megagrams per second squared (Mg/s²). Use a value lower than 50 for an off-road
car, a value between 50 and 100 for a race car and try something around 200 for
something like a Formula 1 car.".TrimStart();
        public static readonly string SuspensionMaxForce = @"
The maximum force the spring can resist. This value should be higher than a
quarter of the mass of the main body or the spring will not carry the weight
of the vehicle. Good results are often obtained by a value that is about 3×
to 4× this number.".TrimStart();
        public static readonly string SuspensionStiffnessByMass = @"
Calculates SuspensionStiffness as a multiplier of vehicle mass".TrimStart();
        public static readonly string SuspensionMaxForceByMass = @"
Calculates SuspensionMaxForce as a multiplier of vehicle mass".TrimStart();

        public static readonly string DampingCompression = @"
The damping applied to the suspension spring when being compressed, meaning
when the wheel is moving up relative to the vehicle. It is measured in
Newton-seconds per millimeter (N⋅s/mm), or megagrams per second (Mg/s).
This value should be between 0.0 (no damping) and 1.0, but may be more.
A value of 0.0 means the car will keep bouncing as the spring keeps its energy.
A good value for this is around 0.3 for a normal car, 0.5 for a race car.".TrimStart();
        public static readonly string DampingRelaxation = @"
The damping applied to the suspension spring when rebounding or extending,
meaning when the wheel is moving down relative to the vehicle. It is measured
in Newton-seconds per millimeter (N⋅s/mm), or megagrams per second (Mg/s).
This value should be between 0.0 (no damping) and 1.0, but may be more.
This value should always be slightly higher than the DampingCompression
property, eg, for a DampingCompression value of 0.3, try a relaxation
value of 0.5.".TrimStart();

        public static readonly string DriftEnabled = @"
Activates DriftSpread on wheels".TrimStart();
        public static readonly string DriftSpread = @"
The amount to adjust WheelFrictionSlip for steer/drive wheels".TrimStart();
    }
}
