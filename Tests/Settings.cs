using System;
using Godot;

namespace F00F.Vehicles.Tests
{
    [Tool]
    public partial class Settings : DataView
    {
        private Label Title => GetNode<Label>("%Title");
        private Button Reset => GetNode<Button>("%Reset");
        private Action OnResetPressed;

        public override void _Ready()
            => Reset.Pressed += () => OnResetPressed?.Invoke();

        public void SetSource(Node3D source)
        {
            if (Init(out var vehicle, out var config))
            {
                SetTitle();
                AddEditControls();
            }

            bool Init(out RigidBody3D vehicle, out VehicleData config)
            {
                OnResetPressed = null;
                Grid.RemoveChildren(free: true);
                return Visible = Init(out vehicle, out config);

                bool Init(out RigidBody3D vehicle, out VehicleData config)
                {
                    vehicle = source as RigidBody3D;
                    config = source?.Get<VehicleData>("Config");
                    return vehicle is not null && config is not null;
                }
            }

            void SetTitle()
                => Title.Text = $"{vehicle.Name} [{vehicle.Mass.Round()}kg]";

            void AddEditControls()
            {
                Grid.Init(config.GetEditControls(out var SetData));
                OnResetPressed += config.Reset;
                SetData(config);
            }
        }
    }
}
