#if DEBUG
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace F00F;

public partial interface IVehicle
{
    float Steer { get; }
    float Drive { get; }
    float Brake { get; }

    Vector3? MeasuredPosition { get; set; }
    Vector3 MeasuredVelocity { get; set; }
    float MeasuredSpeed { get; set; }

    Vector3 GlobalPosition { get; }
}

public partial class GodotVehicle
{
    public float Steer => Steering;
    public float Drive => -EngineForce;

    public Vector3? MeasuredPosition { get; set; }
    public Vector3 MeasuredVelocity { get; set; }
    public float MeasuredSpeed { get; set; }
}

public partial class ArcadeVehicle
{
    public float Steer => default;
    public float Drive => default;
    public float Brake => default;

    public Vector3? MeasuredPosition { get; set; }
    public Vector3 MeasuredVelocity { get; set; }
    public float MeasuredSpeed { get; set; }
}

public partial class RayCastVehicle
{
    public float Steer => default;
    public float Drive => default;
    public float Brake => default;

    public Vector3? MeasuredPosition { get; set; }
    public Vector3 MeasuredVelocity { get; set; }
    public float MeasuredSpeed { get; set; }
}

public partial class RigidBodyVehicle
{
    public float Steer => default;
    public float Drive => default;
    public float Brake => default;

    public Vector3? MeasuredPosition { get; set; }
    public Vector3 MeasuredVelocity { get; set; }
    public float MeasuredSpeed { get; set; }
}

public partial class CharacterBodyVehicle
{
    public float Steer => default;
    public float Drive => default;
    public float Brake => default;

    public Vector3? MeasuredPosition { get; set; }
    public Vector3 MeasuredVelocity { get; set; }
    public float MeasuredSpeed { get; set; }
}

internal static class IVehicleDebug
{
    public static void UpdateDebugInfo(this IVehicle self)
    {
        self.MeasuredVelocity = MeasureVelocity();
        self.MeasuredSpeed = self.MeasuredVelocity.Length();

        Vector3 MeasureVelocity()
        {
            if (self.MeasuredPosition is null)
            {
                self.MeasuredPosition = self.GlobalPosition;
                return Vector3.Zero;
            }

            var curPos = self.GlobalPosition;
            var velocity = curPos - self.MeasuredPosition.Value;
            self.MeasuredPosition = curPos;
            return velocity;
        }
    }

    public static void DisplayDebugInfo(this IVehicle self, Terrain terrain = null, [CallerFilePath] string f = null, [CallerMemberName] string n = null)
    {
        var dbg = ValueWatcher.Instance;
        if (dbg is null) return;

        var body = self as RigidBody3D;
        var wheelCenter = self.WheelCenter();

        dbg.Sep("DEBUG", f: f, n: n);
        dbg.Add(" - Steer", () => self.Steer.Rounded(3), f: f, n: n);
        dbg.Add(" - Drive", () => self.Drive.Rounded(3), f: f, n: n);
        dbg.Add(" - Brake", () => self.Brake.Rounded(3), f: f, n: n);
        dbg.Add(" - WheelCenter", () => $"{wheelCenter.Rounded(3)} ({terrain?.AltitudeDbg(body.ToGlobal(wheelCenter))}m)", f: f, n: n);
        dbg.Add(" - GravityCenter", () => $"{body?.CenterOfMass.Rounded(3)} ({terrain?.AltitudeDbg(body.ToGlobal(body.CenterOfMass))}m)", f: f, n: n);
        dbg.Add(" - VehiclePosition", () => $"{self.GlobalPosition.Rounded(3)} ({terrain?.AltitudeDbg(body.GlobalPosition)}m)", f: f, n: n);
        dbg.Add(" - Speed (measured)", () => $"{self.Speed.Rounded(3)} ({self.MeasuredSpeed.Rounded(3)})", f: f, n: n);
        dbg.Add(" - Velocity (measured)", () => $"{self.Velocity.Rounded(3)} ({self.MeasuredVelocity.Rounded(3)})", f: f, n: n);

        switch (self)
        {
            case GodotVehicle gdv: GDV_DisplayWheelInfo(gdv.GetChildren<VehicleWheel3D>()); break;
                //case ArcadeVehicle acv: ACV_DisplayWheelInfo(acv.GetChildren<VehicleWheel3D>()); break;
                //case RayCastVehicle rcv: RCV_DisplayWheelInfo(rcv.GetChildren<VehicleWheel3D>()); break;
                //case RigidBodyVehicle rbv: RBV_DisplayWheelInfo(rbv.GetChildren<VehicleWheel3D>()); break;
                //case CharacterBodyVehicle cbv: CBV_DisplayWheelInfo(cbv.GetChildren<VehicleWheel3D>()); break;
        }

        void GDV_DisplayWheelInfo(IEnumerable<VehicleWheel3D> wheels)
        {
            wheels.ForEach(x =>
            {
                dbg.Sep($"{x.Name}", f: f, n: n);
                dbg.Add($" - {x.Name} - Steer", () => $"{x.Steering} [{x.UseAsSteering}] (steer)", f: f, n: n);
                dbg.Add($" - {x.Name} - Drive", () => $"{-x.EngineForce} [{x.UseAsTraction}] (drive)", f: f, n: n);
                dbg.Add($" - {x.Name} - Brake", () => $"{x.Brake} (brake)", f: f, n: n);
                dbg.Add($" - {x.Name} - Contact", () => $"{x.GetContactBody()?.Name} [{x.IsInContact()}] (contact)", f: f, n: n);
                dbg.Add($" - {x.Name} - Skid", () => $"{x.GetSkidinfo()} (skid)", f: f, n: n);
                dbg.Add($" - {x.Name} - Rpm", () => $"{x.GetRpm()} (rpm)", f: f, n: n);
            });
        }
    }
}
#endif
