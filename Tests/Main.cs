using System.Linq;
using F00F;
using Godot;

namespace Tests;

using UX = UI;

[Tool]
public partial class Main : Test3D
{
    #region Private

    private Terrain Terrain => field ??= GetNode<Terrain>("Terrain");

    private int VehicleCount { get; set; }

    #endregion

    #region Export

    private const string _VehicleModels = "res://Assets/Vehicles/kenney_car-kit/Vehicles/";
    [Export(PropertyHint.Dir)] private string VehicleModels { get; set; } = _VehicleModels;
    [Export(PropertyHint.Range, "0,100")] private int DropFwd { get; set; } = 10;
    [Export(PropertyHint.Range, "0,100")] private int DropHeight { get; set; } = 25;

    #endregion

    protected sealed override void AddOptions()
    {
        VehicleType type = default;
        AddVehicleOptions();

        // FIXME:  Terrain heights not available until after first frame
        this.CallDeferred(() => this.CallDeferred(ResetVehicles));

        void AddVehicleOptions()
        {
            Options.Sep();
            Options.Add("Vehicles", () => $"({VehicleCount})");
            Options.Add(" - Reset", UX.EnumEdit(type, x => type = x), UX.RightButton(ResetVehicles));
        }

        void ResetVehicles()
        {
            Terrain.RemoveChildren<IVehicle>();
            Camera.SelectMode = false;
            AddVehicles();

            void AddVehicles()
            {
                foreach (var scene in FS.ListRes(VehicleModels).Where(UX.IsSceneFile))
                {
                    var vehicle = IVehicle.New(type);
                    vehicle.Model = new() { Scene = Utils.Load<PackedScene>(scene) };

                    var body = vehicle.Body;

                    var pos = Camera.Position + Camera.Fwd() * DropFwd;
                    pos.Y = Terrain.GetHeight(pos) + DropHeight;
                    body.Position = pos;

                    body.TreeEntered += () => ++VehicleCount;
                    body.TreeExiting += () => --VehicleCount;

                    Terrain.AddChild(body, own: true);
                }
            }
        }
    }

    protected sealed override void InitSettings(out bool ShowSettingsOnCameraSelect)
    {
        Camera.TargetSet += OnTargetSet;
        ShowSettingsOnCameraSelect = false;

        void OnTargetSet()
        {
            Options.Clear();
            Settings.Clear();
            Settings.TitleText = Title();

            if (Camera.Target is IVehicle vehicle)
            {
                InitSettings();
                AddOptions();
            }

            void InitSettings()
            {
                var model = vehicle.Model.Scene;
                Settings.TitleText = Title(vehicle);
                Settings.AddGroup("Model", vehicle.Model, SetVehicleModel);
                Settings.AddGroup("Config", vehicle.Config, x => vehicle.Config = x);
                Settings.AddGroup("Wheels", vehicle.WheelConfig, x => vehicle.WheelConfig = x);
                return;

                void SetVehicleModel(VehicleModel x)
                {
                    x.Scene = model;
                    vehicle.Model = x;
                }
            }

            void AddOptions()
            {
                Options.Add("Current", vehicle.Name.Capitalise);

                if (vehicle is RigidBody3D body)
                {
                    var wheelCenter = vehicle.WheelCenter();
                    Options.Add(" - Mass", () => body.Mass.Rounded(3));
                    Options.Add(" - WheelCenter", () => $"{wheelCenter.Rounded(3)} ({Terrain.Altitude(body.ToGlobal(wheelCenter), 3)}m)");
                    Options.Add(" - GravityCenter", () => $"{body.CenterOfMass.Rounded(3)} ({Terrain.Altitude(body.ToGlobal(body.CenterOfMass), 3)}m)");
                    Options.Add(" - VehiclePosition", () => $"{body.GlobalPosition.Rounded(3)} ({Terrain.Altitude(body.GlobalPosition, 3)}m)");
                }
            }

            string Title(IVehicle node = null)
                => $"Vehicle:  {node?.Name.Capitalise() ?? "<none>"}";
        }
    }
}
