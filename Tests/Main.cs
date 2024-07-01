using Godot;

namespace F00F.Vehicles.Tests
{
    [Tool]
    public partial class Main : Game3D
    {
        public Floor Floor => GetNode<Floor>("Floor");
        public Camera Camera => GetNode<Camera>("Camera");
        public Options Options => GetNode<Options>("Options");
        public Settings Settings => GetNode<Settings>("Settings");

        #region Godot

        public override void _Ready()
        {
            Editor.Disable(this);
            if (Editor.IsEditor) return;

            GD.Print("HI");

            InitSettings();
            InitVehicles();

            void InitSettings()
            {
                ShowSettings(); EnableOptions();
                Camera.TargetSet += ShowSettings;
                Camera.SelectModeSet += EnableOptions;

                void ShowSettings()
                    => Settings.SetSource(Camera.Target);

                void EnableOptions()
                    => Options.EnableOptions(Camera.SelectMode);
            }

            void InitVehicles()
            {
                InitVehicleImport();

                void InitVehicleImport()
                {
                    Options.Import += OnImport;

                    void OnImport(Node x)
                        => AddChild(x, forceReadableName: true);
                }
            }
        }

        public override void _UnhandledKeyInput(InputEvent e)
        {
            if (this.Handle(Input.IsActionJustPressed(MyInput.Quit), Quit)) return;
            if (this.Handle(Input.IsActionJustPressed(MyInput.ToggleFloor), ToggleFloor)) return;
            if (this.Handle(Input.IsActionJustPressed(MyInput.ToggleTarget), ToggleTarget)) return;

            void Quit()
            {
                GetTree().Quit();
                GD.Print("BYE");
            }

            void ToggleFloor()
                => Floor.Visible = !Floor.Visible;

            void ToggleTarget()
            {
                if (Camera.Target is null) return;
                Camera.Target.Visible = !Camera.Target.Visible;
            }
        }

        #endregion
    }
}
