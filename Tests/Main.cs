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

        Options.Sep();
        Options.Add("Vehicles", () => $"({VehicleCount})");
        Options.Add(" - Reset", UX.EnumEdit(type, x => type = x), UX.RightButton(ResetVehicles));

        // FIXME:  Terrain heights not available until after first frame
        this.CallDeferred(() => this.CallDeferred(ResetVehicles));

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

    protected sealed override void InitSettings()
    {
        Camera.TargetSet += OnTargetSet;

        void OnTargetSet()
        {
            Settings.Clear();

            if (Camera.Target is IVehicle vehicle)
            {
                var model = vehicle.Model.Scene;
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
        }
    }
}
