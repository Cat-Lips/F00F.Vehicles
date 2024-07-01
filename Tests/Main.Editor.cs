#if TOOLS
namespace F00F.Vehicles.Tests
{
    public partial class Main
    {
        public override void _Notification(int what)
        {
            if (Editor.OnPreSave(what))
            {
                Editor.DoPreSaveReset(Camera, Camera.PropertyName._input);
                Editor.DoPreSaveReset(Camera, Camera.PropertyName._config);
                return;
            }

            if (Editor.OnPostSave(what))
                Editor.DoPostSaveRestore();
        }
    }
}
#endif
