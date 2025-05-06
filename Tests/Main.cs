using System.Linq;
using F00F;
using Game;
using Godot;

namespace Tests;

using UX = UI;

[Tool]
public partial class Main : Test3D
{
    private Terrain Terrain => field ??= GetNode<Terrain>("Terrain");
    private TrackEditor TrackEditor => field ??= GetNode<TrackEditor>("TrackEditor");

    private const string _VehicleModels = "res://Assets/Vehicles/kenney_car-kit/Vehicles/";
    [Export(PropertyHint.Dir)] private string VehicleModels { get; set; } = _VehicleModels;
    [Export(PropertyHint.Range, "0,100")] private int DropFwd { get; set; } = 10;
    [Export(PropertyHint.Range, "0,100")] private int DropHeight { get; set; } = 25;

    protected sealed override void InitInput()
        => VehicleInput.Init();

    protected sealed override void InitSettings(out bool ShowSettingsOnCameraSelect)
    {
        Camera.TargetSet += OnTargetSet;
        ShowSettingsOnCameraSelect = false;

        void OnTargetSet()
        {
            Settings.Clear();
            Settings.TitleText = Title();

            if (Camera.Target is IVehicle vehicle)
            {
                var model = vehicle.Model.Scene;
                Settings.TitleText = Title(vehicle);
                Settings.AddGroup("Model", vehicle.Model, SetVehicleModel);
                Settings.AddGroup("Config", vehicle.Config, x => vehicle.Config = x);
                Settings.AddGroup("Wheels", vehicle.WheelConfig, x => vehicle.WheelConfig = x);

                void SetVehicleModel(VehicleModel x)
                {
                    x.Scene = model;
                    vehicle.Model = x;
                }
            }

            string Title(IVehicle node = null)
                => $"Vehicle:  {node?.Name.Capitalise() ?? "<none>"}";
        }
    }

    protected sealed override void InitOptions()
    {
        InitTrackEditor();
        InitVehicleOptions();
        DisplayVehicleInfo();

        void InitTrackEditor()
            => TrackEditor.Initialise();

        void InitVehicleOptions()
        {
            var count = 0;
            var type = VehicleType.RayCast;

            Options.Sep();
            Options.Add("Vehicles", () => $"({count})");
            Options.Add(" - Reset", UX.EnumEdit(type, x => type = x), UX.RightButton(ResetVehicles));

            // FIXME:  Terrain heights not available until after first frame
            this.CallDeferred(() => this.CallDeferred(ResetVehicles));

            void ResetVehicles()
            {
                Terrain.RemoveChildren<IVehicle>();
                GetViewport().GuiReleaseFocus();
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

                        body.TreeEntered += () => ++count;
                        body.TreeExiting += () => --count;

                        Terrain.AddChild(body, own: true);
                    }
                }
            }
        }
    }

    private void DisplayVehicleInfo()
    {
        Camera.TargetSet += OnTargetSet;

        void OnTargetSet()
        {
            Options.Clear();

            if (Camera.Target is IVehicle vehicle)
            {
                Options.Sep();
                Options.Add("Vehicle", vehicle.Name.Capitalise);
                Options.Add(" - Mass", () => vehicle.Mass.Rounded(3));
                Options.Add(" - Speed", () => vehicle.Speed.Rounded(3));
                Options.Add(" - Grounded", () => vehicle.Grounded);
#if DEBUG
                vehicle.DisplayDebugInfo(Terrain);
#endif
            }
        }
    }
}
