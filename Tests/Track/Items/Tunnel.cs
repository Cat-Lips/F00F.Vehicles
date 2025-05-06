using Godot;

namespace Game;

[Tool]
public partial class Tunnel : Node3D, ITrackItem
{
    private CsgTorus3D Shape => field ??= GetNode<CsgTorus3D>("Shape");

    public void ShowMesh(bool show) => Shape.Visible = show;
    public void EnableShape(bool enable) => Shape.UseCollision = enable;
}
