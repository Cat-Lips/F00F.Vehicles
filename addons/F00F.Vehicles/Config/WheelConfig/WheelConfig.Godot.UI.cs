namespace F00F;

public partial class WheelConfig : IEditable<WheelConfig>
{
    private static void GDV_UI(UI.IBuilder ui)
    {
        ui.AddGroup("Wheels", "GDV_Wheel");
        ui.AddValue(nameof(GDV_WheelRollDamp), range: (null, null, null));
        ui.AddValue(nameof(GDV_WheelFrictionSlip), range: (null, null, null));
        ui.AddValue(nameof(GDV_WheelRestLength), range: (null, null, null));
        ui.EndGroup();
        ui.AddGroup("Suspension", "GDV_Suspension");
        ui.AddValue(nameof(GDV_SuspensionTravel), range: (null, null, null));
        ui.AddValue(nameof(GDV_SuspensionStrength), range: (null, null, null));
        ui.AddValue(nameof(GDV_SuspensionStiffness), range: (null, null, null));
        ui.EndGroup();
        ui.AddGroup("Damping", "GDV_Damping");
        ui.AddValue(nameof(GDV_DampingCompression), range: (null, null, null));
        ui.AddValue(nameof(GDV_DampingRelaxation), range: (null, null, null));
        ui.EndGroup();
        ui.AddGroup("Drift", "GDV_Drift");
        ui.AddCheck(nameof(GDV_DriftEnabled));
        ui.AddValue(nameof(GDV_DriftSpread), range: (null, null, null));
        ui.EndGroup();
    }
}
