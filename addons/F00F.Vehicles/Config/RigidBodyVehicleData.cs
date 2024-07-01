using System.Linq;
using Godot;

namespace F00F.Vehicles
{
    [Tool, GlobalClass]
    public partial class RigidBodyVehicleData : VehicleData
    {
        public void Init(RigidBodyVehicle root)
        {
            InitModel(root, VehicleModel.WheelShapeType.Cylinder, VehicleModel.BodyShapeType.Convex, OnModelChanged: InitWheels);

            void InitWheels()
            {
                Model.Wheels.Cast<CollisionShape3D>().ForEach(InitWheel);

                void InitWheel(CollisionShape3D shape)
                {

                }
            }
        }

        public void OnProcess(bool active, float delta)
        {
        }

        public void OnPhysicsProcess(bool active, float delta)
        {
        }
    }
}
