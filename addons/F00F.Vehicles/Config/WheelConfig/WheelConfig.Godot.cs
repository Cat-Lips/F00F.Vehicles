using System;
using System.Collections.Generic;
using Godot;

namespace F00F;

public partial class WheelConfig
{
    #region Defaults

    private static readonly VehicleWheel3D GDV_Default = new()
    {
        WheelRollInfluence = 0f,
        WheelFrictionSlip = 10.5f,
        WheelRestLength = 0.15f,

        SuspensionTravel = 10f,
        SuspensionMaxForce = 6000f,
        SuspensionStiffness = 40f,

        DampingCompression = 1.9f,
        DampingRelaxation = 2f,
    };

    #endregion

    #region Export

    [ExportGroup("Wheels", "GDV_Wheel")]
    [Export] public float GDV_WheelRollInfluence { get; set => this.Set(ref field, value, GDV_WheelRollInfluenceSet); } = GDV_Default.WheelRollInfluence;
    [Export] public float GDV_WheelFrictionSlip { get; set => this.Set(ref field, value, GDV_WheelFrictionSlipSet); } = GDV_Default.WheelFrictionSlip;
    [Export] public float GDV_WheelRestLength { get; set => this.Set(ref field, value, GDV_WheelRestLengthSet); } = GDV_Default.WheelRestLength;

    [ExportGroup("Suspension", "GDV_Suspension")]
    [Export] public float GDV_SuspensionTravel { get; set => this.Set(ref field, value, GDV_SuspensionTravelSet); } = GDV_Default.SuspensionTravel;
    [Export] public float GDV_SuspensionMaxForce { get; set => this.Set(ref field, value, GDV_SuspensionMaxForceSet); } = GDV_Default.SuspensionMaxForce;
    [Export] public float GDV_SuspensionStiffness { get; set => this.Set(ref field, value, GDV_SuspensionStiffnessSet); } = GDV_Default.SuspensionStiffness;
    [Export] public float GDV_SuspensionMaxForceByMass { get; set => this.Set(ref field, value, GDV_SuspensionMaxForceByMassSet); }
    [Export] public float GDV_SuspensionStiffnessByMass { get; set => this.Set(ref field, value, GDV_SuspensionStiffnessByMassSet); }

    [ExportGroup("Damping", "GDV_Damping")]
    [Export] public float GDV_DampingCompression { get; set => this.Set(ref field, value, GDV_DampingCompressionSet); } = GDV_Default.DampingCompression;
    [Export] public float GDV_DampingRelaxation { get; set => this.Set(ref field, value, GDV_DampingRelaxationSet); } = GDV_Default.DampingRelaxation;

    [ExportGroup("Drift", "GDV_Drift")]
    [Export] public bool GDV_DriftEnabled { get; set => this.Set(ref field, value, GDV_DriftEnabledSet); }
    [Export] public float GDV_DriftSpread { get; set => this.Set(ref field, value, GDV_DriftSpreadSet); }

    private Action GDV_WheelRollInfluenceSet;
    private Action GDV_WheelFrictionSlipSet;
    private Action GDV_WheelRestLengthSet;

    private Action GDV_SuspensionTravelSet;
    private Action GDV_SuspensionMaxForceSet;
    private Action GDV_SuspensionStiffnessSet;
    private Action GDV_SuspensionMaxForceByMassSet;
    private Action GDV_SuspensionStiffnessByMassSet;

    private Action GDV_DampingCompressionSet;
    private Action GDV_DampingRelaxationSet;

    private Action GDV_DriftEnabledSet;
    private Action GDV_DriftSpreadSet;

    #endregion

    public void Init(GodotVehicle root, IEnumerable<VehicleWheel3D> wheels)
    {
        Init(root);
        InitWheels();
        InitActions();

        void InitWheels()
        {
            SetWheelRollInfluence();
            SetWheelFrictionSlip();
            SetWheelRestLength();

            SetSuspensionTravel();
            SetSuspensionMaxForce();
            SetSuspensionStiffness();

            SetDampingCompression();
            SetDampingRelaxation();
        }

        void InitActions()
        {
            GDV_WheelRollInfluenceSet = SetWheelRollInfluence;
            GDV_WheelFrictionSlipSet = SetWheelFrictionSlip;
            GDV_WheelRestLengthSet = SetWheelRestLength;

            GDV_SuspensionTravelSet = SetSuspensionTravel;
            GDV_SuspensionMaxForceSet = SetSuspensionMaxForce;
            GDV_SuspensionStiffnessSet = SetSuspensionStiffness;
            GDV_SuspensionMaxForceByMassSet = SetSuspensionMaxForceByMass;
            GDV_SuspensionStiffnessByMassSet = SetSuspensionStiffnessByMass;

            GDV_DampingCompressionSet = SetDampingCompression;
            GDV_DampingRelaxationSet = SetDampingRelaxation;

            GDV_DriftEnabledSet = SetWheelFrictionSlip;
            GDV_DriftSpreadSet = SetWheelFrictionSlip;
        }

        void SetWheelRollInfluence() => wheels.ForEach(x => x.WheelRollInfluence = GDV_WheelRollInfluence);
        void SetWheelFrictionSlip() => wheels.ForEach(x => x.WheelFrictionSlip = GetDrift(x));
        void SetWheelRestLength() => wheels.ForEach(x => x.WheelRestLength = GDV_WheelRestLength);

        void SetSuspensionTravel() => wheels.ForEach(x => x.SuspensionTravel = GDV_SuspensionTravel);

        void SetDampingCompression() => wheels.ForEach(x => x.DampingCompression = GDV_DampingCompression);
        void SetDampingRelaxation() => wheels.ForEach(x => x.DampingRelaxation = GDV_DampingRelaxation);

        void SetSuspensionMaxForce()
        {
            wheels.ForEach(x => x.SuspensionMaxForce = GDV_SuspensionMaxForce);
            GDV_SuspensionMaxForceByMass = (GDV_SuspensionMaxForce / root.Mass).Rounded(3);
        }

        void SetSuspensionStiffness()
        {
            wheels.ForEach(x => x.SuspensionStiffness = GDV_SuspensionStiffness);
            GDV_SuspensionStiffnessByMass = (GDV_SuspensionStiffness / root.Mass).Rounded(3);
        }

        void SetSuspensionMaxForceByMass()
            => GDV_SuspensionMaxForce = (GDV_SuspensionMaxForceByMass * root.Mass).Rounded(3);

        void SetSuspensionStiffnessByMass()
            => GDV_SuspensionStiffness = (GDV_SuspensionStiffnessByMass * root.Mass).Rounded(3);

        float GetDrift(VehicleWheel3D wheel)
        {
            // FIXME:  Need to make spread a ratio of distance between drive/steer wheels
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
}
