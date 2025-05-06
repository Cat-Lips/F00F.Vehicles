using Godot;

namespace Game;

public interface ITrackItem
{
    Vector3 Scale { get; set; }
    Vector3 Position { get; set; }
    Vector3 Rotation { get; set; }
    Transform3D Transform { get; set; }
    void Reparent(Node newParent, bool keepGlobalTransform = true);

    void ShowMesh(bool show);
    void EnableShape(bool enable);
}
