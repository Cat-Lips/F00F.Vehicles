namespace F00F;

public partial class WheelConfig
{
    private static void RCV_UI(UI.IBuilder ui)
    {
        ui.AddGroup("Suspension", "RCV_");
        ui.AddValue(nameof(RCV_SpringStrength), range: (0, null, null));
        ui.AddValue(nameof(RCV_SpringDamping), range: (0, 1, null));
        ui.AddValue(nameof(RCV_SpringLength), range: (0, null, null));
        ui.EndGroup();
    }
}
