#if TOOLS
using Godot;

namespace F00F;

public partial class RayCastVehicle
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
                Editor.DoPreSaveResetField(this, PropertyName.WheelConfig);
            }

            Editor.DoPreSaveReset(this, PropertyName.Mass);
            Editor.DoPreSaveReset(this, PropertyName.Freeze);
            Editor.DoPreSaveResetMeta(this, GLB.Aabb, GLB.Mass);
            this.ForEachChild(x => Editor.DoPreSaveReset(x, Node.PropertyName.Owner));
            OnEditorSave();
            return;
        }

        if (Editor.OnPostSave(what))
            Editor.DoPostSaveRestore();
    }
}
#endif
