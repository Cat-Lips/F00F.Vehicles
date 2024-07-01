using System;
using Godot;

// TODO: https://www.asawicki.info/Mirror/Car%20Physics%20for%20Games/Car%20Physics%20for%20Games.html
// https://medium.com/@Satyam_Mishra/a-simple-guide-to-script-your-first-movable-car-in-godot-engine-93177db71472
// TODO: https://www.youtube.com/watch?v=CdPYlj5uZeI

namespace F00F.Vehicles
{
    [Tool, GlobalClass]
    public partial class VehiclePhysics : Resource
    {
        private float _Brake;
        public float Brake { get => _Brake; private set => this.Set(ref _Brake, value, BrakeSet); }
        public event Action BrakeSet;

        private float _Steer;
        public float Steer { get => _Steer; private set => this.Set(ref _Steer, value, SteerSet); }
        public event Action SteerSet;

        private float _Drive;
        public float Drive { get => _Drive; private set => this.Set(ref _Drive, value, DriveSet); }
        public event Action DriveSet;

        private float _MaxDrive;
        public float MaxDrive { get => _MaxDrive; private set => this.Set(ref _MaxDrive, value, MaxDriveSet); }
        public event Action MaxDriveSet;

        public virtual void Update(RigidBody3D vehicle, VehicleInput input, VehicleSettings config, float delta, bool active)
        {
            UpdateSteer();
            UpdateDrive();

            void UpdateSteer()
            {
                var steerInput = active ? input.GetSteer() : 0;
                var steerTarget = steerInput * config.MaxSteer;
                Steer = Mathf.MoveToward(Steer, steerTarget, config.SteerSpeed * delta);
            }

            void UpdateDrive()
            {
                var driveInput = active ? input.GetDrive() : 0;

                if (driveInput is 0) ApplyDeceleration();
                else if (driveInput > 0) ApplyAcceleration();
                else if (IsMovingForward()) ApplyBrake();
                else ApplyReverse();

                void ApplyDeceleration()
                {
                    Brake = 0;
                    MaxDrive = 0;
                    Drive = Mathf.MoveToward(Drive, MaxDrive, config.Deceleration * delta);
                }

                void ApplyAcceleration()
                {
                    Brake = 0;
                    var turbo = input.Turbo();
                    MaxDrive = turbo ? config.MaxTurbo : config.MaxSpeed;
                    var acceleration = driveInput * (turbo ? config.Turbo : config.Acceleration);
                    Drive = Mathf.MoveToward(Drive, MaxDrive, acceleration * delta);
                }

                void ApplyBrake()
                {
                    Brake = driveInput * config.Brake;
                    MaxDrive = 0;
                    Drive = 0;
                }

                void ApplyReverse()
                {
                    Brake = 0;
                    MaxDrive = -config.MaxReverse;
                    var acceleration = driveInput * config.Reverse;
                    Drive = Mathf.MoveToward(Drive, MaxDrive, acceleration * delta);
                }

                bool IsMovingForward()
                    => vehicle.IsMovingForward(vehicle.LinearVelocity);
            }
        }
    }
}
