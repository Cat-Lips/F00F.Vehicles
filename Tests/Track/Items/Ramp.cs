using F00F;
using Godot;

namespace Game;

[Tool]
public partial class Ramp : StaticBody3D, ITrackItem
{
    private MeshInstance3D Mesh => field ??= GetNode<MeshInstance3D>("Mesh");
    private CollisionShape3D Shape => field ??= GetNode<CollisionShape3D>("Shape");

    public void ShowMesh(bool show) => Mesh.Visible = show;
    public void EnableShape(bool enable) => Shape.Enabled(enable);
}
