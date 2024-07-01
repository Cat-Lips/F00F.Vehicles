using System;
using Godot;

namespace F00F.Vehicles.Tests
{
    [Tool]
    public partial class Options : Stats
    {
        public event Action<Node> Import;

        protected override void OnReady()
        {
            base.OnReady();

            AddSep();
            AddImport();
            AddSep();

            void AddImport()
            {
                Action<Error, string> SetFileError = null;
                var (root, _, _, btn, _) = UI.NewFileSelect("Import", out var SetFilePath, out SetFileError, Import, filter: ["*.glb", "*.gltf"]);
                _EnableOptions += active => btn.Disabled = !active;
                Add("ImportVehicle", root);

                void Import(string path)
                {
                    if (!GLB.Load(path, out var scene, out var err, out var msg))
                    {
                        Utils.ShowError(this, err, msg);
                        SetFileError(err, msg);
                        return;
                    }

                    GlbEditWindow.Instantiate(this, scene, this.Import);
                }
            }
        }

        private Action<bool> _EnableOptions;
        public void EnableOptions(bool enable)
            => _EnableOptions?.Invoke(enable);
    }
}
