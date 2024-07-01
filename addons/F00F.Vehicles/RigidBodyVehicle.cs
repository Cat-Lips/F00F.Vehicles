using System;
using Godot;

namespace F00F.Vehicles
{
    [Tool]
    public partial class RigidBodyVehicle : RigidBody3D, IActive, ITarget
    {
        private bool _active;
        public bool Active { get => _active; set => this.Set(ref _active, value, ActiveSet); }
        public event Action ActiveSet;

        private RigidBodyVehicleData _config;
        [Export] public RigidBodyVehicleData Config { get => _config; set => this.Set(ref _config, value ?? new(), () => Config.Init(this)); }

        public override void _Ready()
        {
            Config ??= new();

            Editor.Disable(this);
            if (Editor.IsEditor) return;
        }

        public override void _Process(double delta)
            => Config.OnProcess(Active, (float)delta);

        public override void _PhysicsProcess(double delta)
            => Config.OnPhysicsProcess(Active, (float)delta);
    }
}
