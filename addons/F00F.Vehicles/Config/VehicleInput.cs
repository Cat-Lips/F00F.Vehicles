using Godot;

namespace F00F.Vehicles
{
    [Tool, GlobalClass]
    public partial class VehicleInput : Resource
    {
        protected internal virtual bool Reset()
            => Input.IsActionJustPressed(MyInput.Reset);

        protected internal virtual bool Drift()
            => Input.IsActionPressed(MyInput.Drift);

        protected internal virtual bool Turbo()
            => Input.IsActionPressed(MyInput.Turbo);

        protected internal virtual bool Bounce()
            => Input.IsActionJustPressed(MyInput.Bounce);

        protected internal virtual float GetDrive()
            => Input.GetAxis(MyInput.Brake, MyInput.Accelerate);

        protected internal virtual float GetSteer()
            => Input.GetAxis(MyInput.SteerLeft, MyInput.SteerRight);
    }
}
