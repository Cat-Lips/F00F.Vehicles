using System;
using System.Linq;
using Godot;

namespace F00F.Vehicles
{
    [Tool, GlobalClass]
    public partial class ArcadeVehicleData : VehicleData
    {
        #region Export

        [ExportGroup("Sphere")]

        private float _Radius;
        [Export(PropertyHint.Range, "0,2,or_greater")] public float Radius { get => _Radius; set => this.Set(ref _Radius, value, RadiusSet); }
        private event Action RadiusSet;

        private float _Bounce = .1f;
        [Export(PropertyHint.Range, "0,1")] public float Bounce { get => _Bounce; set => this.Set(ref _Bounce, value, BounceSet); }
        private event Action BounceSet;

        private float _Friction = .9f;
        [Export(PropertyHint.Range, "0,1")] public float Friction { get => _Friction; set => this.Set(ref _Friction, value, FrictionSet); }
        private event Action FrictionSet;

        [ExportGroup("Features")]

        [Export(PropertyHint.Range, "0,45,or_greater")] public float BodyTilt { get; set; } = 10; // Smaller = more tilt
        [Export(PropertyHint.Range, "0,10,or_greater")] public float TurnSpeed { get; set; } = 4;

        #endregion

        public Vector3 MeshOffset { get; private set; }

        public void Init(ArcadeVehicle root)
        {
            InitModel(root, OnModelChanged: () => { InitMesh(); InitSphere(); });
            RadiusSet += InitSphere;
            InitSettings();
            InitPhysics();

            void InitMesh()
                => root.ForEachChild<MeshInstance3D>(x => x.Reparent(root.Mesh));

            void InitSphere()
            {
                var radius = Radius is 0 ? ModelRadius() : (Radius * Model.Scale);
                var wheelRadius = Vector3.Down * WheelRadius();
                MeshOffset = Vector3.Down * radius;

                root.Sphere.Radius = radius;
                root.Ray.TargetPosition = MeshOffset + wheelRadius;

                float ModelRadius()
                    => Model.Aabb.GetShortestAxisSize() * .5f;

                float WheelRadius()
                    => Model.Wheels.Count is 0 ? .5f : Model.Wheels.Average(x => ((Aabb)x.GetMeta("Aabb")).Size.Y) * .5f;
            }

            void InitSettings()
            {
                InitSettings();
                SettingsSet += InitSettings;

                void InitSettings()
                {
                    SetAngularDamp();
                    Settings.DecelerationSet += SetAngularDamp;

                    void SetAngularDamp()
                        => root.AngularDamp = Settings.Deceleration;
                }
            }

            void InitPhysics()
            {
                root.PhysicsMaterialOverride ??= new();

                SetBounce();
                SetFriction();
                BounceSet += SetBounce;
                FrictionSet += SetFriction;

                void SetBounce() => root.PhysicsMaterialOverride.Bounce = Bounce;
                void SetFriction() => root.PhysicsMaterialOverride.Friction = Friction;
            }
        }
    }
}
