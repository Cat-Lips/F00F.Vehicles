using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace F00F.Vehicles
{
    [Tool, GlobalClass]
    public partial class VehicleModel : Resource
    {
        #region Enums

        public enum WheelShapeType
        {
            None = GlbShapeType.None,
            Convex = GlbShapeType.Convex,
            SimpleConvex = GlbShapeType.SimpleConvex,
            Cylinder = GlbShapeType.CylinderX,
            RayShape = GlbShapeType.RayShapeY,
            RayCast = GlbShapeType.RayCastY,
            Wheel = GlbShapeType.Wheel,
        }

        public enum BodyShapeType
        {
            None = GlbShapeType.None,
            Convex = GlbShapeType.Convex,
            MultiConvex = GlbShapeType.MultiConvex,
            SimpleConvex = GlbShapeType.SimpleConvex,
            Box = GlbShapeType.Box,
        }

        public enum PartsShapeType
        {
            None = GlbShapeType.None,
            MultiConvex = GlbShapeType.MultiConvex,
        }

        #endregion

        #region Export

        private PackedScene _Model;
        [Export] public PackedScene Model { get => _Model; set => this.Set(ref _Model, value, ResetModel.Run); }

        private bool _Flip = true;
        [Export] public bool Flip { get => _Flip; set => this.Set(ref _Flip, value, ResetModel.Run); }

        private float _Mass = 1;
        [Export(PropertyHint.Range, "0,1,or_greater")] public float Mass { get => _Mass; set => this.Set(ref _Mass, value, ResetModel.Run); }

        private float _Scale = 1;
        [Export(PropertyHint.Range, "0,1,or_greater")] public float Scale { get => _Scale; set => this.Set(ref _Scale, value, ResetModel.Run); }

        #endregion

        public event Action ModelChanged;

        public Aabb Aabb { get; private set; }
        public List<Node3D> Body { get; } = [];
        public List<Node3D> Parts { get; } = [];
        public List<Node3D> Wheels { get; } = [];

        public void Init(PhysicsBody3D root, WheelShapeType wheels = default, BodyShapeType body = default, PartsShapeType parts = default, Node3D target = null)
        {
            PurgeParts(Root);
            Root = root;

            BodyShape = (GlbShapeType)body;
            PartsShape = (GlbShapeType)parts;
            WheelShape = (GlbShapeType)wheels;

            ResetModel.Run();
        }

        #region Private

        private PhysicsBody3D Root;
        private GlbShapeType BodyShape;
        private GlbShapeType WheelShape;
        private GlbShapeType PartsShape;

        private AutoAction ResetModel { get; } = new();

        private static void PurgeParts(Node root)
        {
            if (IsInstanceValid(root))
            {
                root.RecurseChildren()
                    .Where(x => x.Owner is null)
                    .ForEach(x => x.QueueFree());
            }
        }

        public VehicleModel()
        {
            this.ResetModel.Action += ResetModel;
            this.ResetModel.Action += ModelChanged;

            void ResetModel()
            {
                ClearParts();
                AddParts();
                SetAabb();

                void ClearParts()
                {
                    Body.Clear();
                    Parts.Clear();
                    Wheels.Clear();
                    PurgeParts(Root);
                }

                void AddParts()
                {
                    GLB.AddParts(
                        own: false,
                        Root, Model?.Instantiate(),
                        Flip ? GlbFrontFace.NegZ : GlbFrontFace.Z,
                        Mass, Scale, null, GetShapeType, null, null, OnPartAdded);

                    void OnPartAdded(MeshInstance3D source, Node3D part)
                    {
                        MyParts().Add(part);

                        List<Node3D> MyParts() =>
                            IsWheel(source) ? Wheels :
                            IsBodyPart(source) ? Body :
                            Parts;
                    }

                    GlbShapeType GetShapeType(MeshInstance3D source) =>
                        IsWheel(source) ? WheelShape :
                        IsBodyPart(source) ? BodyShape :
                        PartsShape;

                    static bool IsWheel(MeshInstance3D source)
                        => source.GetParentOrNull<MeshInstance3D>() is null && source.Name.ContainsN("wheel");

                    static bool IsBodyPart(MeshInstance3D source)
                        => source.GetParentOrNull<MeshInstance3D>() is null;
                }

                void SetAabb()
                    => Aabb = (Aabb)Root.GetMeta("Aabb");
            }

            void ModelChanged()
                => this.ModelChanged?.Invoke();
        }

        #endregion
    }
}
