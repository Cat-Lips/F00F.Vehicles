using System;
using System.Linq;
using Godot;

namespace F00F.Vehicles
{
    [Tool, GlobalClass]
    public partial class GodotVehicleData : VehicleData
    {
        #region Defaults

        private static class Default
        {
            public const float WheelRollInfluence = 1;
            public const float WheelRestLength = 0;
            public const float WheelFrictionSlip = 1;

            public const float SuspensionTop = 0;
            public const float SuspensionTravel = 0;
            public const float SuspensionStiffness = 100; // 50-200
            public const float SuspensionForceMultiplier = 10;

            public const float DampingCompression = .3f;
            public const float DampingRelaxation = .5f;
        }

        #endregion

        #region Export

        private float _WheelRollInfluence = Default.WheelRollInfluence;
        private float _WheelRestLength = Default.WheelRestLength;
        private float _WheelFrictionSlip = Default.WheelFrictionSlip;
        private float _SuspensionTop = Default.SuspensionTop;
        private float _SuspensionTravel = Default.SuspensionTravel;
        private float _SuspensionStiffness = Default.SuspensionStiffness;
        private float _SuspensionForceMultiplier = Default.SuspensionForceMultiplier;
        private float _DampingCompression = Default.DampingCompression;
        private float _DampingRelaxation = Default.DampingRelaxation;

        [ExportGroup("Wheels", "Wheel")]
        [Export(PropertyHint.Range, "0,1")] public float WheelRollInfluence { get => _WheelRollInfluence; set => this.Set(ref _WheelRollInfluence, value, WheelRollInfluenceSet); }
        [Export] public float WheelRestLength { get => _WheelRestLength; set => this.Set(ref _WheelRestLength, value, WheelRestLengthSet); }
        [Export(PropertyHint.Range, "0,1")] public float WheelFrictionSlip { get => _WheelFrictionSlip; set => this.Set(ref _WheelFrictionSlip, value, WheelFrictionSlipSet); }

        [ExportGroup("Suspension", "Suspension")]
        [Export(PropertyHint.Range, "0,.5")] public float SuspensionTop { get => _SuspensionTop; set => this.Set(ref _SuspensionTop, value, SuspensionTopSet); }
        [Export(PropertyHint.Range, "0,.5")] public float SuspensionTravel { get => _SuspensionTravel; set => this.Set(ref _SuspensionTravel, value, SuspensionTravelSet); }
        [Export] public float SuspensionStiffness { get => _SuspensionStiffness; set => this.Set(ref _SuspensionStiffness, value, SuspensionStiffnessSet); }
        [Export] public float SuspensionForceMultiplier { get => _SuspensionForceMultiplier; set => this.Set(ref _SuspensionForceMultiplier, value, SuspensionForceMultiplierSet); }

        [ExportGroup("Damping", "Damping")]
        [Export(PropertyHint.Range, "0,1")] public float DampingCompression { get => _DampingCompression; set => this.Set(ref _DampingCompression, value, DampingCompressionSet); }
        [Export(PropertyHint.Range, "0,1")] public float DampingRelaxation { get => _DampingRelaxation; set => this.Set(ref _DampingRelaxation, value, DampingRelaxationSet); }

        private event Action WheelRollInfluenceSet;
        private event Action WheelRestLengthSet;
        private event Action WheelFrictionSlipSet;
        private event Action SuspensionTopSet;
        private event Action SuspensionTravelSet;
        private event Action SuspensionStiffnessSet;
        private event Action SuspensionForceMultiplierSet;
        private event Action DampingCompressionSet;
        private event Action DampingRelaxationSet;

        #endregion

        #region Reset

        public override void Reset()
        {
            base.Reset();

            WheelRollInfluence = Default.WheelRollInfluence;
            WheelRestLength = Default.WheelRestLength;
            WheelFrictionSlip = Default.WheelFrictionSlip;
            SuspensionTop = Default.SuspensionTop;
            SuspensionTravel = Default.SuspensionTravel;
            SuspensionStiffness = Default.SuspensionStiffness;
            SuspensionForceMultiplier = Default.SuspensionForceMultiplier;
            DampingCompression = Default.DampingCompression;
            DampingRelaxation = Default.DampingRelaxation;
        }

        #endregion

        #region Init

        public void Init(GodotVehicle root)
        {
            InitModel(root, VehicleModel.WheelShapeType.Wheel, VehicleModel.BodyShapeType.Convex, OnModelChanged: InitWheels);

            void InitWheels()
            {
                Model.Wheels.Cast<VehicleWheel3D>().ForEach(InitWheel);

                void InitWheel(VehicleWheel3D wheel)
                {
                    var origin = wheel.Position;

                    wheel.WheelRollInfluence = WheelRollInfluence;
                    wheel.WheelRestLength = WheelRestLength;
                    wheel.WheelFrictionSlip = WheelFrictionSlip;
                    wheel.Position = origin + SuspensionTop * Vector3.Up;
                    wheel.SuspensionTravel = SuspensionTravel;
                    wheel.SuspensionStiffness = SuspensionStiffness;
                    wheel.SuspensionMaxForce = SuspensionMaxForce();
                    wheel.DampingCompression = DampingCompression;
                    wheel.DampingRelaxation = DampingRelaxation;

                    WheelRollInfluenceSet += () => wheel.WheelRollInfluence = WheelRollInfluence;
                    WheelRestLengthSet += () => wheel.WheelRestLength = WheelRestLength;
                    WheelFrictionSlipSet += () => wheel.WheelFrictionSlip = WheelFrictionSlip;
                    SuspensionTopSet += () => wheel.Position = origin + SuspensionTop * Vector3.Up;
                    SuspensionTravelSet += () => wheel.SuspensionTravel = SuspensionTravel;
                    SuspensionStiffnessSet += () => wheel.SuspensionStiffness = SuspensionStiffness;
                    SuspensionForceMultiplierSet += () => wheel.SuspensionMaxForce = SuspensionMaxForce();
                    DampingCompressionSet += () => wheel.DampingCompression = DampingCompression;
                    DampingRelaxationSet += () => wheel.DampingRelaxation = DampingRelaxation;

                    float SuspensionMaxForce() => root.Mass / Model.Wheels.Count * SuspensionForceMultiplier;
                }
            }
        }

        #endregion

        public void OnProcess(bool active, float delta)
        {
        }

        public void OnPhysicsProcess(bool active, float delta)
        {
        }
    }
}
