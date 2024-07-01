using System;
using System.Linq;
using Godot;
using static F00F.Vehicles.VehicleModel;

namespace F00F.Vehicles
{
    [Tool]
    public abstract partial class VehicleData : Resource
    {
        #region Export

        private VehicleModel _Model = new();
        [Export] public VehicleModel Model { get => _Model; set => this.Set(ref _Model, value ?? new(), ModelSet); }
        protected event Action ModelSet;

        private VehicleSettings _settings = new();
        [Export] public VehicleSettings Settings { get => _settings; set => this.Set(ref _settings, value ?? new(), SettingsSet); }
        protected event Action SettingsSet;

        private VehicleInput _input = new();
        public VehicleInput Input { get => _input; set => this.Set(ref _input, value ?? new()); }

        private VehiclePhysics _physics = new();
        public VehiclePhysics Physics { get => _physics; set => this.Set(ref _physics, value ?? new()); }

        #endregion

        public Node3D[] SteerWheels { get; private set; }
        public Node3D[] DriveWheels { get; private set; }

        public void UpdatePhysics(RigidBody3D root, float delta, bool active)
            => Physics.Update(root, Input, Settings, delta, active);

        public virtual void Reset()
        {
            Model = new();
            Settings = new();
        }

        #region Protected

        private Action InitModelAction;
        private Action ModelChangedAction;
        protected void InitModel(RigidBody3D root, WheelShapeType wheels = default, BodyShapeType body = default, PartsShapeType parts = default, Action OnModelChanged = null)
        {
            InitModel();
            InitModelAction = InitModel;
            ModelChangedAction = OnModelChanged;

            void InitModel()
                => Model.Init(root, wheels, body, parts);
        }

        protected VehicleData()
        {
            ModelSet += OnModelSet;
            SettingsSet += OnSettingsSet;

            void OnModelSet()
            {
                InitModelAction?.Invoke();
                Model.ModelChanged += OnModelChanged;

                void OnModelChanged()
                {
                    SetSteerWheels();
                    SetDriveWheels();
                    ModelChangedAction?.Invoke();
                }
            }

            void OnSettingsSet()
            {
                SetSteerWheels();
                SetDriveWheels();
                Settings.SteerTypeSet += SetSteerWheels;
                Settings.DriveTypeSet += SetDriveWheels;
            }

            void SetSteerWheels() => SteerWheels = Model.Wheels.Where(Settings.IsSteerWheel).ToArray();
            void SetDriveWheels() => DriveWheels = Model.Wheels.Where(Settings.IsDriveWheel).ToArray();
        }

        #endregion
    }
}
