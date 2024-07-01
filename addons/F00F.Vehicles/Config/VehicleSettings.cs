using System;
using Godot;

namespace F00F.Vehicles
{
    [Tool, GlobalClass]
    public partial class VehicleSettings : Resource
    {
        public enum WheelAction
        {
            All,
            Back,
            Front,
        }

        private float _Turbo = 300;
        private float _Brake = 400;
        private float _Bounce = 10;
        private float _Reverse = 100;
        private float _SteerSpeed = 3;
        private float _Acceleration = 100;
        private float _Deceleration = 200;

        private float _MaxSpeed = 1000;
        private float _MaxTurbo = 2000;
        private float _MaxSteer = Const.Deg30;
        private float _MaxReverse = 1000;

        private WheelAction _DriveType = WheelAction.Back;
        private WheelAction _SteerType = WheelAction.Front;

        [ExportGroup("Physics")]
        [Export] public float Turbo { get => _Turbo; set => this.Set(ref _Turbo, value, TurboSet); }
        [Export] public float Brake { get => _Brake; set => this.Set(ref _Brake, value, BrakeSet); }
        [Export] public float Bounce { get => _Bounce; set => this.Set(ref _Bounce, value, BounceSet); }
        [Export] public float Reverse { get => _Reverse; set => this.Set(ref _Reverse, value, ReverseSet); }
        [Export] public float SteerSpeed { get => _SteerSpeed; set => this.Set(ref _SteerSpeed, value, SteerSpeedSet); }
        [Export] public float Acceleration { get => _Acceleration; set => this.Set(ref _Acceleration, value, AccelerationSet); }
        [Export] public float Deceleration { get => _Deceleration; set => this.Set(ref _Deceleration, value, DecelerationSet); }

        [ExportSubgroup("MaxValues")]
        [Export] public float MaxSpeed { get => _MaxSpeed; set => this.Set(ref _MaxSpeed, value, MaxSpeedSet); }
        [Export] public float MaxTurbo { get => _MaxTurbo; set => this.Set(ref _MaxTurbo, value, MaxTurboSet); }
        [Export(PropertyHint.Range, "0,90,radians_as_degrees")] public float MaxSteer { get => _MaxSteer; set => this.Set(ref _MaxSteer, value, MaxSteerSet); }
        [Export] public float MaxReverse { get => _MaxReverse; set => this.Set(ref _MaxReverse, value, MaxReverseSet); }

        [ExportGroup("Features")]
        [Export] public WheelAction DriveType { get => _DriveType; set => this.Set(ref _DriveType, value, DriveTypeSet); }
        [Export] public WheelAction SteerType { get => _SteerType; set => this.Set(ref _SteerType, value, SteerTypeSet); }

        public event Action TurboSet;
        public event Action BrakeSet;
        public event Action BounceSet;
        public event Action ReverseSet;
        public event Action SteerSpeedSet;
        public event Action AccelerationSet;
        public event Action DecelerationSet;

        public event Action MaxSpeedSet;
        public event Action MaxTurboSet;
        public event Action MaxSteerSet;
        public event Action MaxReverseSet;

        public event Action DriveTypeSet;
        public event Action SteerTypeSet;
    }
}
