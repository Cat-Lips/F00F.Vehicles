using System;
using System.Collections.Generic;
using Godot;

namespace F00F;

using ControlPair = (Control Label, Control EditControl);

public partial class VehicleConfig : IEditable<VehicleConfig>
{
    public IEnumerable<ControlPair> GetEditControls() => GetEditControls(out var _);
    public IEnumerable<ControlPair> GetEditControls(out Action<VehicleConfig> SetData)
    {
        var ec = EditControls(out SetData, VehicleType);
        SetData(this);
        return ec;
    }

    public static IEnumerable<ControlPair> EditControls(out Action<VehicleConfig> SetData, VehicleType type)
    {
        return UI.Create(out SetData, CreateUI);

        void CreateUI(UI.IBuilder ui)
        {
            ui.AddOption(nameof(DriveType), items: UI.Items<WheelAction>());
            ui.AddOption(nameof(SteerType), items: UI.Items<WheelAction>());

            switch (type)
            {
                case VehicleType.Godot: GDV_UI(ui); break;
                case VehicleType.Arcade: AV_UI(ui); break;
                case VehicleType.RayCast: RCV_UI(ui); break;
                case VehicleType.RigidBody: RBV_UI(ui); break;
                case VehicleType.CharacterBody: CBV_UI(ui); break;
                default: throw new NotImplementedException();
            }

            ui.AddValue(nameof(JumpStrength), range: (0, null, null));

            void GDV_UI(UI.IBuilder ui)
            {
                ui.AddGroup(nameof(GodotVehicle), "GDV_");
                ui.AddValue(nameof(GDV_Engine), range: (0, null, null));
                ui.AddValue(nameof(GDV_Brake), range: (0, 1, null));
            }

            void AV_UI(UI.IBuilder ui)
            {
            }

            void RCV_UI(UI.IBuilder ui)
            {
            }

            void RBV_UI(UI.IBuilder ui)
            {
            }

            void CBV_UI(UI.IBuilder ui)
            {
            }
        }
    }
}
