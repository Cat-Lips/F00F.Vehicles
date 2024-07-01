using System;
using Godot;

namespace F00F.Vehicles
{
    [Tool]
    public partial class ArcadeVehicle : RigidBody3D, IActive, ITarget
    {
        private Node3D _mesh;
        private RayCast3D _ray;
        private SphereShape3D _sphere;
        private CollisionShape3D _shape;

        internal Node3D Mesh => _mesh ??= GetNode<Node3D>("Mesh");
        internal RayCast3D Ray => _ray ??= GetNode<RayCast3D>("Ray");
        internal SphereShape3D Sphere => _sphere ??= (SphereShape3D)Shape.Shape;
        internal CollisionShape3D Shape => _shape ??= GetNode<CollisionShape3D>("Shape");

        private bool _active;
        public bool Active { get => _active; set => this.Set(ref _active, value, ActiveSet); }
        public event Action ActiveSet;

        private float _speed;
        public float Speed { get => _speed; private set => this.Set(ref _speed, value, SpeedSet); }
        public event Action SpeedSet;

        private ArcadeVehicleData _config;
        [Export] public ArcadeVehicleData Config { get => _config; set => this.Set(ref _config, value ?? new(), () => Config.Init(this)); }

        public bool Grounded => Ray.IsColliding();
        public Vector3 GroundNormal => Ray.GetCollisionNormal();
        public Node3D Target => Mesh;

        #region Godot

        public override void _Ready()
        {
            Config ??= new();

            Editor.Disable(this);
            if (Editor.IsEditor) return;
        }

        public override void _Process(double delta)
        {
            Speed = LinearVelocity.Length();
            UpdateVehicle((float)delta);

            void UpdateVehicle(float delta)
            {
                var body = Config.Model.Body;
                var steer = Config.Physics.Steer;
                //var drive = Config.Physics.Drive;

                //RotateSteerWheels();
                //RotateDriveWheels();

                if (Grounded)
                {
                    TiltBody();
                    TurnMesh();
                    AlignMesh();
                }

                void TiltBody()
                    => body.ForEach(x => x.Rotation = x.Rotation.With(z: steer * Speed / Config.BodyTilt));

                void TurnMesh()
                    => Mesh.RotateY(steer * Mathf.Min(Speed, Config.TurnSpeed) * delta);

                void AlignMesh()
                    => Mesh.GlobalTransform = Mesh.GlobalTransform.Align(GroundNormal);

                //void RotateSteerWheels()
                //    => Config.SteerWheels.ForEach(x => x.Rotation = x.Rotation.With(y: steer));

                //void RotateDriveWheels()
                //    => Config.DriveWheels.ForEach(x => x.RotateObjectLocal(Vector3.ModelLeft, drive));
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            UpdatePhysics((float)delta);

            void UpdatePhysics(float delta)
            {
                Config.UpdatePhysics(this, delta, Active);

                Mesh.Position = GlobalPosition + Config.MeshOffset;

                if (Active && Grounded)
                    ApplyCentralForce(Mesh.Forward() * Config.Physics.Drive);
            }
        }

        #endregion
    }
}
