using System;
using System.Collections.Generic;
using Godot;

namespace F00F;

public partial class WheelConfig
{
    #region Defaults

    private static class GDV_Default
    {
        public const float WheelRollDamp = 1f;
        public const float WheelRestLength = 0.15f;
        public const float WheelFrictionSlip = 10.5f;

        public const float SuspensionTravel = 10f;
        public const float SuspensionStrength = 4f;
        public const float SuspensionStiffness = .4f;

        public const float DampingCompression = 1.9f;
        public const float DampingRelaxation = 2f;
    }

    #endregion

    #region Actions

    private Action GDV_WheelRollDampSet;
    private Action GDV_WheelRestLengthSet;
    private Action GDV_WheelFrictionSlipSet;

    private Action GDV_SuspensionTravelSet;
    private Action GDV_SuspensionStrengthSet;
    private Action GDV_SuspensionStiffnessSet;

    private Action GDV_DampingCompressionSet;
    private Action GDV_DampingRelaxationSet;

    private Action GDV_DriftEnabledSet;
    private Action GDV_DriftSpreadSet;

    #endregion

    #region Export

    [ExportGroup("Wheels", "GDV_Wheel")]
    [Export] public float GDV_WheelRollDamp { get; set => this.Set(ref field, value, GDV_WheelRollDampSet); } = GDV_Default.WheelRollDamp;
    [Export] public float GDV_WheelRestLength { get; set => this.Set(ref field, value, GDV_WheelRestLengthSet); } = GDV_Default.WheelRestLength;
    [Export] public float GDV_WheelFrictionSlip { get; set => this.Set(ref field, value, GDV_WheelFrictionSlipSet); } = GDV_Default.WheelFrictionSlip;

    [ExportGroup("Suspension", "GDV_Suspension")]
    [Export] public float GDV_SuspensionTravel { get; set => this.Set(ref field, value, GDV_SuspensionTravelSet); } = GDV_Default.SuspensionTravel;
    [Export] public float GDV_SuspensionStrength { get; set => this.Set(ref field, value, GDV_SuspensionStrengthSet); } = GDV_Default.SuspensionStrength;
    [Export] public float GDV_SuspensionStiffness { get; set => this.Set(ref field, value, GDV_SuspensionStiffnessSet); } = GDV_Default.SuspensionStiffness;

    [ExportGroup("Damping", "GDV_Damping")]
    [Export] public float GDV_DampingCompression { get; set => this.Set(ref field, value, GDV_DampingCompressionSet); } = GDV_Default.DampingCompression;
    [Export] public float GDV_DampingRelaxation { get; set => this.Set(ref field, value, GDV_DampingRelaxationSet); } = GDV_Default.DampingRelaxation;

    [ExportGroup("Drift", "GDV_Drift")]
    [Export] public bool GDV_DriftEnabled { get; set => this.Set(ref field, value, GDV_DriftEnabledSet); }
    [Export] public float GDV_DriftSpread { get; set => this.Set(ref field, value, GDV_DriftSpreadSet); }

    #endregion

    #region Init

    public void Init(GodotVehicle vehicle, IEnumerable<VehicleWheel3D> wheels)
    {
        Init(vehicle);
        InitWheels();
        SetActions();

        void InitWheels()
        {
            SetWheelRollDamp();
            SetWheelRestLength();
            SetWheelFrictionSlip();

            SetSuspensionTravel();
            SetSuspensionStrength();
            SetSuspensionStiffness();

            SetDampingCompression();
            SetDampingRelaxation();
        }

        void SetActions()
        {
            GDV_WheelRollDampSet = SetWheelRollDamp;
            GDV_WheelRestLengthSet = SetWheelRestLength;
            GDV_WheelFrictionSlipSet = SetWheelFrictionSlip;

            GDV_SuspensionTravelSet = SetSuspensionTravel;
            GDV_SuspensionStrengthSet = SetSuspensionStrength;
            GDV_SuspensionStiffnessSet = SetSuspensionStiffness;

            GDV_DampingCompressionSet = SetDampingCompression;
            GDV_DampingRelaxationSet = SetDampingRelaxation;

            GDV_DriftEnabledSet = SetWheelFrictionSlip;
            GDV_DriftSpreadSet = SetWheelFrictionSlip;
        }

        void SetWheelRollDamp() => wheels.ForEach(x => x.WheelRollInfluence = GDV_WheelRollDamp);
        void SetWheelRestLength() => wheels.ForEach(x => x.WheelRestLength = GDV_WheelRestLength);
        void SetWheelFrictionSlip() => wheels.ForEach(x => x.WheelFrictionSlip = GetDrift(x));

        void SetSuspensionTravel() => wheels.ForEach(x => x.SuspensionTravel = GDV_SuspensionTravel);
        void SetSuspensionStrength() => wheels.ForEach(x => x.SuspensionMaxForce = GDV_SuspensionStrength * vehicle.Mass);
        void SetSuspensionStiffness() => wheels.ForEach(x => x.SuspensionStiffness = GDV_SuspensionStiffness * vehicle.Mass);

        void SetDampingCompression() => wheels.ForEach(x => x.DampingCompression = GDV_DampingCompression);
        void SetDampingRelaxation() => wheels.ForEach(x => x.DampingRelaxation = GDV_DampingRelaxation);

        float GetDrift(VehicleWheel3D wheel)
        {
            return GDV_WheelFrictionSlip + GetDriftSpread();

            float GetDriftSpread()
            {
                return
                    !GDV_DriftEnabled ? 0 :
                    IsSteerWheel() ? GDV_DriftSpread :
                    IsDriveWheel() ? -GDV_DriftSpread :
                    0;

                bool IsSteerWheel() => wheel.UseAsSteering && !wheel.UseAsTraction;
                bool IsDriveWheel() => wheel.UseAsTraction && !wheel.UseAsSteering;
            }
        }
    }

    #endregion
}
