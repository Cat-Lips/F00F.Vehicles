#if TOOLS
using F00F;
using Godot.Collections;

namespace Tests;

public partial class Main
{
    public sealed override void _ValidateProperty(Dictionary property)
    {
        if (this.IsEditedSceneRoot())
        {
            if (Editor.SetDisplayOnly(property, PropertyName.DropFwd)) return;
            if (Editor.SetDisplayOnly(property, PropertyName.DropHeight)) return;
            if (Editor.SetDisplayOnly(property, PropertyName.VehicleModels)) return;
        }
    }

    protected sealed override void _OnEditorSave()
    {
        Editor.DoPreSaveReset(Camera, Camera3D.PropertyName.Position);
        Editor.DoPreSaveReset(Camera, Camera3D.PropertyName.Near, .05f);
        Editor.DoPreSaveReset(Camera, Camera3D.PropertyName.Far, 4000);

        Editor.DoPreSaveResetField(Terrain, Terrain.PropertyName.Config);
        Editor.DoPreSaveResetField(Terrain, Terrain.PropertyName.Camera);
    }
}
#endif
