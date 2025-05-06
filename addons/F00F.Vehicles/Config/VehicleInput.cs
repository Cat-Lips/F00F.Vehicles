using Godot;

namespace F00F;

[Tool, GlobalClass]
public partial class VehicleInput : CustomResource
{
    protected internal virtual float Steer()
        => MyInput.GetAxis(MyInput.SteerRight, MyInput.SteerLeft);

    protected internal virtual float Drive()
        => MyInput.GetAxis(MyInput.Forward, MyInput.Reverse);

    protected internal virtual bool Jump()
        => MyInput.IsActionPressed(MyInput.Jump);

    protected internal virtual bool Brake()
        => MyInput.IsActionPressed(MyInput.Brake);

    protected internal virtual bool Turbo1()
        => MyInput.IsActionPressed(MyInput.Turbo1);

    protected internal virtual bool Turbo2()
        => MyInput.IsActionPressed(MyInput.Turbo2);
}
