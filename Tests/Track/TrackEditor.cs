using System;
using F00F;
using Godot;

namespace Game;

using Camera = F00F.Camera3D;

[Tool]
public partial class TrackEditor : Node
{
    #region Private

    private Node Parent => field ??= GetParent();
    private Viewport Viewport => field ??= GetViewport();
    private Node3D Anchor => field ??= GetNode<Node3D>("Anchor");
    private Camera Camera => field ??= (Camera)Viewport.GetCamera3D();
    private Terrain Terrain => field ??= Parent.GetNode<Terrain>("Terrain");
    private Options Options => field ??= Parent.GetNode<Options>("UI/Options");

    private bool CanPlace { get; set; }
    private ITrackItem CurEditItem { get; set; }
    private Action<float> SetEditScale { get; set; }
    private Action<TrackItemType> SetEditItem { get; set; }
    private bool Flipped { get; set; }

    #endregion

    public bool EditorActive => Camera.Target.IsNull();
    public bool PlacementActive => CurEditItem.NotNull();

    public enum TrackItemType { None, Ramp, Tunnel }
    public enum PlacementStyle { AlignToWorld, AlignToView }
    public TrackItemType EditItem { get; private set => this.Set(ref field, value, OnEditItemSet); }
    public PlacementStyle Placement { get; private set => this.Set(ref field, value, OnPlacementSet); } = PlacementStyle.AlignToView;
    public float EditScale { get; private set => this.Set(ref field, value, OnEditScaleSet); } = 10;

    public void Initialise()
    {
        var ecItem = UI.EnumEdit(EditItem, x => EditItem = x);
        var ecPlace = UI.EnumEdit(Placement, x => Placement = x);
        var ecScale = UI.ValueEdit(EditScale, x => EditScale = x);
        var ecClear = UI.CloseButton(ClearTrack);
        Camera.TargetSet += EnableEditControls;
        SetEditScale = x => ecScale.Value = x;
        SetEditItem = x => ecItem.Select((int)x);

        Options.Sep();
        Options.Add("Track Item", ecItem);
        Options.Add(" - Placement", ecPlace);
        Options.Add(" - Scale", ecScale);
        Options.Add(" - Clear", UI.Label(null), ecClear); // FIXME:  Button big without spacer!

        EnableEditControls();
        SetProcess(PlacementActive);

        void ClearTrack()
        {
            GetViewport().GuiReleaseFocus();
            this.RemoveChildren<ITrackItem>();
        }

        void EnableEditControls()
        {
            EnableEditControls(EditorActive);

            void EnableEditControls(bool enable)
            {
                ecItem.Enabled(enable);
                ecPlace.Enabled(enable);
                ecScale.Enabled(enable);
                ecClear.Enabled(enable);

                if (!enable)
                    CurEditItem?.ShowMesh(CanPlace = false);
            }
        }
    }

    #region Godot

    #region Process (Ray)

    public sealed override void _Process(double _)
    {
        if (!EditorActive) return;
        if (!PlacementActive) return;

        if (TryGetCloserTerrain(out var pos))
            AlignPlacement();
        else DenyPlacement();

        bool TryGetCloserTerrain(out Vector3 pos)
        {
            if (!MyInput.Active) { pos = default; return false; }
            var range = HitTarget?.GlobalPosition.DistanceTo(RayStart) ?? Camera.Far;
            return Terrain.CastRay(RayStart, RayNormal, out pos, range);
        }

        void AlignPlacement()
        {
            CurEditItem.ShowMesh(CanPlace = true);
            Anchor.Transform = Placement switch
            {
                PlacementStyle.AlignToWorld => Terrain.Align(pos, pos + Vector3.Forward),
                PlacementStyle.AlignToView => Terrain.Align(pos, Camera.Position),
                _ => throw new NotImplementedException(),
            };
        }

        void DenyPlacement()
            => CurEditItem.ShowMesh(CanPlace = false);
    }

    private Vector3 RayStart { get; set; }
    private Vector3 RayNormal { get; set; }
    private Node3D HitTarget { get; set; }
    public sealed override void _PhysicsProcess(double _)
    {
        if (!EditorActive) return;
        if (!PlacementActive) return;

        TryGetHitTarget();

        void TryGetHitTarget()
        {
            var screenPos = Viewport.GetMousePosition();
            RayStart = Camera.ProjectRayOrigin(screenPos);
            RayNormal = Camera.ProjectRayNormal(screenPos);
            HitTarget = Camera.RayCastHitBody(RayStart, RayNormal, Camera.Far);
            if (HitTarget is TerrainShape) HitTarget = null;
        }
    }

    #endregion

    #region Input (Edit)

    public sealed override void _UnhandledInput(InputEvent _)
    {
        if (HitTarget is ITrackItem &&
            this.Handle(MyInput.IsActionJustPressed(MyInput.Remove), RemoveItem)) return;

        if (!CanPlace) return;
        if (!PlacementActive) return;

        if (this.Handle(MyInput.IsActionJustPressed(MyInput.Place), PlaceItem)) return;
        if (this.Handle(MyInput.IsActionJustPressed(MyInput.FlipFwd), FlipItem)) return;
        if (this.Handle(MyInput.IsActionJustPressed(MyInput.ScaleUp), ScaleUpItem)) return;
        if (this.Handle(MyInput.IsActionJustPressed(MyInput.ScaleDown), ScaleDownItem)) return;

        void PlaceItem()
        {
            var xform = CurEditItem.Transform;

            CurEditItem.EnableShape(true);
            CurEditItem.Reparent(this);
            CurEditItem = null;
            OnEditItemSet();

            CurEditItem.Transform = xform;
        }

        void RemoveItem()
            => HitTarget.DetachChild(free: true);

        void FlipItem()
        {
            if (Alt) { FaceLeft(); return; }
            if (Ctrl) { FaceRight(); return; }

            Flip(Flipped = !Flipped);
        }

        void ScaleUpItem()
        {
            if (Alt) { RotateLeft(); return; }
            if (Ctrl) { RotateRight(); return; }

            SetEditScale(EditScale + 1);
        }

        void ScaleDownItem()
        {
            if (Ctrl) { RotateLeft(); return; }
            if (Alt) { RotateRight(); return; }

            SetEditScale(EditScale - 1);
        }

        void Flip(bool flip)
        {
            var rot = CurEditItem.Rotation;
            rot.Y = flip ? Const.Deg180 : 0;
            CurEditItem.Rotation = rot;
        }

        void FaceLeft()
        {
            var rot = CurEditItem.Rotation;
            rot.Y = Const.Deg90;
            CurEditItem.Rotation = rot;
        }

        void FaceRight()
        {
            var rot = CurEditItem.Rotation;
            rot.Y = -Const.Deg90;
            CurEditItem.Rotation = rot;
        }

        void RotateLeft()
        {
            var rot = CurEditItem.Rotation;
            rot.Y += Const.Deg15;
            CurEditItem.Rotation = rot;
        }

        void RotateRight()
        {
            var rot = CurEditItem.Rotation;
            rot.Y -= Const.Deg15;
            CurEditItem.Rotation = rot;
        }
    }

    private bool Alt { get; set; }
    private bool Ctrl { get; set; }
    public sealed override void _UnhandledKeyInput(InputEvent _e)
    {
        var e = (InputEventKey)_e;

        if (e.Echo) return;
        if (e.IsKey(Key.Alt)) { Alt = e.Pressed; return; }
        if (e.IsKey(Key.Ctrl)) { Ctrl = e.Pressed; return; }
    }

    #endregion

    #endregion

    #region Private

    private void OnEditItemSet()
    {
        (CurEditItem as Node)?.DetachChild(free: true);
        CurEditItem = NewTrackItemOrNull();
        SetProcess(PlacementActive);

        ITrackItem NewTrackItemOrNull() => EditItem switch
        {
            TrackItemType.None => null,
            TrackItemType.Ramp => Utils.New<Ramp>(InitItem),
            TrackItemType.Tunnel => Utils.New<Tunnel>(InitItem),
            _ => throw new NotImplementedException(),
        };

        void InitItem(ITrackItem x)
        {
            x.ShowMesh(false);
            x.EnableShape(false);
            x.Scale = Vector3.One * EditScale;
            Anchor.AddChild((Node)x);
        }
    }

    private void OnPlacementSet()
    {

    }

    private void OnEditScaleSet()
    {
        if (CurEditItem.IsNull()) return;
        CurEditItem.Scale = Vector3.One * EditScale;
    }

    #endregion
}
