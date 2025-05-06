using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace F00F;

using ControlPair = (Control Label, Control EditControl);

public partial class VehicleModel : IEditable<VehicleModel>
{
    public sealed override IEnumerable<ControlPair> GetEditControls() => GetEditControls(out var _);
    public IEnumerable<ControlPair> GetEditControls(out Action<VehicleModel> SetData)
    {
        var ec = EditControls(out SetData);
        SetData(this);
        return ec;
    }

    public static IEnumerable<ControlPair> EditControls(out Action<VehicleModel> SetData)
    {
        OptionButton BodyShapes = null;
        OptionButton PartShapes = null;
        OptionButton WheelShapes = null;

        var ecParent = PhysicsConfig.EditControls(out var SetBaseData);
        var ecSelf = UI.Create(out Action<VehicleModel> SetMyData, CreateUI, CustomiseUI);
        SetData = x => { SetBaseData(x); UpdateUI(x); SetMyData(x); };
        return ecParent.Concat(ecSelf);

        static void CreateUI(UI.IBuilder ui)
        {
            ui.AddOption(nameof(BodyShape), items: UI.ItemIds(Default.AllowedBodyShapes));
            ui.AddOption(nameof(PartsShape), items: UI.ItemIds(Default.AllowedPartShapes));
            ui.AddOption(nameof(WheelShape), items: UI.ItemIds(Default.AllowedWheelShapes));
        }

        void CustomiseUI(UI.IBuilder ui)
        {
            BodyShapes = ui.GetEditControl<OptionButton>(nameof(BodyShape));
            PartShapes = ui.GetEditControl<OptionButton>(nameof(PartsShape));
            WheelShapes = ui.GetEditControl<OptionButton>(nameof(WheelShape));

            UpdateUI(null);
        }

        void UpdateUI(VehicleModel x)
        {
            BodyShapes.EnableItems(x?.AllowedBodyShapes);
            PartShapes.EnableItems(x?.AllowedPartShapes);
            WheelShapes.EnableItems(x?.AllowedWheelShapes);
        }
    }
}
