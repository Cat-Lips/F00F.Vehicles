﻿using System.Linq;
using Godot;

namespace F00F.Vehicles
{
    [Tool, GlobalClass]
    public partial class RayCastVehicleData : VehicleData
    {
        public void Init(RayCastVehicle root)
        {
            InitModel(root, VehicleModel.WheelShapeType.RayCast, VehicleModel.BodyShapeType.Convex, OnModelChanged: InitWheels);

            void InitWheels()
            {
                Model.Wheels.Cast<RayCast3D>().ForEach(InitWheel);

                void InitWheel(RayCast3D wheel)
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