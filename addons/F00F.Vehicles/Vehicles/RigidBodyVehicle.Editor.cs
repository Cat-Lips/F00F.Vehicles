#if TOOLS
using Godot;

namespace F00F;

public partial class RigidBodyVehicle
{
    protected virtual void OnEditorSave() { }
    public sealed override void _Notification(int what)
    {
        if (Editor.OnPreSave(what))
        {
            if (this.IsEditedSceneRoot())
            {
                Editor.DoPreSaveResetField(this, PropertyName.Model);
                Editor.DoPreSaveResetField(this, PropertyName.Config);
            }

            Editor.DoPreSaveResetMeta(this, GLB.Aabb, GLB.Mass);
            this.ForEachChild(x => Editor.DoPreSaveReset(x, Node.PropertyName.Owner));
            Editor.DoPreSaveReset(this, PropertyName.Mass, 40);
            Editor.DoPreSaveReset(this, PropertyName.Freeze);
            OnEditorSave();
            return;
        }

        if (Editor.OnPostSave(what))
            Editor.DoPostSaveRestore();
    }
}
#endif
