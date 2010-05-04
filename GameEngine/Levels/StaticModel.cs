// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaticModel.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The static model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;

    using FarseerGames.FarseerPhysics.Collisions;
    using FarseerGames.FarseerPhysics.Factories;

    using Gdd.Game.Engine.Levels.Characters;
    using Gdd.Game.Engine.Physics;
    using Gdd.Game.Engine.Render;
    using Gdd.Game.Engine.Scenes;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The static model.
    /// </summary>
    public class StaticModel : DrawableSceneComponent
    {
        #region Constants and Fields

        /// <summary>
        /// The offset.
        /// </summary>
        public Vector2 offset;

        /// <summary>
        /// The add offset.
        /// </summary>
        protected bool AddOffset;

        /// <summary>
        /// The grid cell size.
        /// </summary>
        protected float gridCellSize = -1.0f;

        /// <summary>
        /// The mass.
        /// </summary>
        protected float mass;

        /// <summary>
        /// The model name.
        /// </summary>
        protected string modelName;

        /// <summary>
        /// The rotation along the X axis
        /// </summary>
        protected float pitchRotation;

        /// <summary>
        /// The rotation along the X axis
        /// </summary>
        protected float rollRotation;

        /// <summary>
        /// The rotation along the X axis
        /// </summary>
        protected float yawRotation;

        /// <summary>
        /// The is updating.
        /// </summary>
        private bool isUpdating;

        /// <summary>
        /// The physics vertices.
        /// </summary>
        private Vertices physicsVertices;

        /// <summary>
        /// The scale.
        /// </summary>
        private Vector2 scale;

        /// <summary>
        /// The scale changed.
        /// </summary>
        private bool scaleChanged;

        /// <summary>
        /// Gets or sets ModelTexture.
        /// </summary>
        private List<Texture2D> textures;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticModel"/> class.
        /// </summary>
        /// <param name="game">
        /// The game instance.
        /// </param>
        public StaticModel(Game game)
            : base(game)
        {
            this.AddOffset = true;
            this.modelName = "tmpCube";
            this.YawRotation = MathHelper.PiOver2;
            this.PitchRotation = 0.0f;
            this.RollRotation = 0.0f;
            this.scale = Vector2.One;

            this.Rotation = Matrix.CreateFromYawPitchRoll(this.YawRotation, this.PitchRotation, this.RollRotation);

            this.ModelDirection = ModelDirection.Right;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The geometry type of the object
        /// </summary>
        public GeometryType GeometryType { get; set; }

        /// <summary>
        /// Gets or sets Direction.
        /// </summary>
        public ModelDirection ModelDirection { get; protected set; }

        /// <summary>
        /// Gets or sets ModelName.
        /// </summary>
        public string ModelName
        {
            get
            {
                return this.modelName;
            }

            set
            {
                this.modelName = value;
                if (this.isLoaded)
                {
                    this.LoadContent();
                    this.isChanged = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets ObjectModel.
        /// </summary>
        public Model ObjectModel { get; protected set; }

        /// <summary>
        /// Gets or sets PhysicsBodyRotation.
        /// </summary>
        public float PhysicsBodyRotation { get; set; }

        /// <summary>
        /// Gets or sets PhysicsTexture.
        /// </summary>
        public Texture2D PhysicsTexture { get; protected set; }

        /// <summary>
        /// Gets or sets PhysicsVertices.
        /// </summary>
        public Vertices PhysicsVertices
        {
            get
            {
                return this.physicsVertices;
            }

            protected set
            {
                this.physicsVertices = value;
            }
        }

        /// <summary>
        /// Gets or sets PitchRotation.
        /// </summary>
        public float PitchRotation
        {
            get
            {
                return this.pitchRotation;
            }

            set
            {
                this.pitchRotation = value;
                this.Rotation = Matrix.CreateFromYawPitchRoll(this.yawRotation, this.pitchRotation, this.rollRotation);
            }
        }

        /// <summary>
        /// Gets or sets Position2D.
        /// </summary>
        public override Vector2 Position2D
        {
            get
            {
                return base.Position2D;
            }

            set
            {
                base.Position2D = value;
                if (this.PhysicsBody != null && !this.isUpdating && this.AddOffset)
                {
                    this.PhysicsBody.Position = value + this.offset;
                }
                else if (this.PhysicsBody != null && !this.isUpdating && !this.AddOffset)
                {
                    this.PhysicsBody.Position = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets RollRotation.
        /// </summary>
        public float RollRotation
        {
            get
            {
                return this.rollRotation;
            }

            set
            {
                this.rollRotation = value;
                this.Rotation = Matrix.CreateFromYawPitchRoll(this.yawRotation, this.pitchRotation, this.rollRotation);
            }
        }

        /// <summary>
        /// Gets or sets Scale.
        /// </summary>
        public Vector2 Scale
        {
            get
            {
                return this.scale;
            }

            set
            {
                if (Math.Abs(this.scale.X - value.X) > 0.01 || Math.Abs(this.scale.Y - value.Y) > 0.01)
                {
                    this.scaleChanged = true;
                }

                this.scale = value;
            }
        }

        /// <summary>
        /// Gets or sets ScaleMatrix.
        /// </summary>
        public Matrix ScaleMatrix { get; set; }

        /// <summary>
        /// Gets or sets YawRotation.
        /// </summary>
        public float YawRotation
        {
            get
            {
                return this.yawRotation;
            }

            set
            {
                this.yawRotation = value;
                this.Rotation = Matrix.CreateFromYawPitchRoll(this.yawRotation, this.pitchRotation, this.rollRotation);
            }
        }

        /// <summary>
        /// Gets or sets ModelTextures.
        /// </summary>
        protected List<Texture2D> ModelTextures
        {
            get
            {
                return this.textures ?? (this.textures = new List<Texture2D>());
            }

            set
            {
                this.textures = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The get model vertices.
        /// </summary>
        /// <param name="objectModel">
        /// The object model.
        /// </param>
        /// <returns>
        /// </returns>
        public static Vector3[] GetModelVertices(Model objectModel)
        {
            int sizeOfVertex = GetSizeOfMesh(objectModel);

            var vertices = new List<Vector3>();

            foreach (ModelMesh mesh in objectModel.Meshes)
            {
                var meshBuffer = new MyStruct[mesh.VertexBuffer.SizeInBytes / sizeOfVertex];

                mesh.VertexBuffer.GetData(meshBuffer);
                IEnumerable<Vector3> vertexBuffer = from v in meshBuffer
                                                    select new Vector3(v.Position.Z, v.Position.Y, v.Position.X);

                vertices.AddRange(vertexBuffer);
            }

            return vertices.ToArray();
        }

        /// <summary>
        /// The get size of mesh.
        /// </summary>
        /// <param name="objectModel">
        /// The object model.
        /// </param>
        /// <returns>
        /// The get size of mesh.
        /// </returns>
        public static int GetSizeOfMesh(Model objectModel)
        {
            int sizeofVertex = 0;
            foreach (ModelMesh modelMesh in objectModel.Meshes)
            {
                foreach (var vertexElements in
                    modelMesh.MeshParts.Select(modelMeshPart => modelMeshPart.VertexDeclaration.GetVertexElements()))
                {
                    foreach (VertexElement vertexElement in vertexElements)
                    {
                        switch (vertexElement.VertexElementFormat)
                        {
                            case VertexElementFormat.Vector3:
                                sizeofVertex += Marshal.SizeOf(typeof(Vector3));
                                break;
                            case VertexElementFormat.Vector2:
                                sizeofVertex += Marshal.SizeOf(typeof(Vector2));
                                break;
                        }
                    }

                    break;
                }
            }

            return sizeofVertex;
        }

        /// <summary>
        /// The draw with technique.
        /// </summary>
        /// <param name="effect">
        /// The effect.
        /// </param>
        /// <param name="technique">
        /// The technique.
        /// </param>
        public override void DrawWithEffect(ShaderManager.EFFECT_ID effect, string technique)
        {
            // code from Riemers XNA tutorial
            foreach (ModelMesh mesh in this.ObjectModel.Meshes)
            {
                int count = 0;
                foreach (ModelMeshPart mmp in mesh.MeshParts)
                {
                    if (count < this.ModelTextures.Count)
                    {
                        ShaderManager.SetValue("Texture", this.ModelTextures[count++]);
                    }

                    ShaderManager.SetValue("ID", this.ID);
                    ShaderManager.SetValue("life", Hero.GetHeroLife());
                    ShaderManager.SetValue("World", this.World);

                    ShaderManager.CommitChanges();
                    mmp.Effect = ShaderManager.GetEffect(effect);
                }

                foreach (Effect e in mesh.Effects)
                {
                    e.CurrentTechnique = e.Techniques[technique];
                }

                mesh.Draw();
            }
        }

        /// <summary>
        /// The set texture.
        /// </summary>
        /// <param name="texture">
        /// The texture.
        /// </param>
        public void SetTexture(Texture2D texture)
        {
            this.ModelTextures.Add(texture);
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Update(GameTime gameTime)
        {
            this.isUpdating = true;
            if (this.GeometryType == GeometryType.Circle)
            {
                float circleScale = this.scale.Length();
                this.ScaleMatrix = Matrix.CreateScale(circleScale, -circleScale, circleScale);
            }
            else
            {
                this.ScaleMatrix = Matrix.CreateScale(1, this.scale.Y, this.scale.X);
            }

            if (this.PhysicsBody != null)
            {
                this.PhysicsBody.Rotation += this.PhysicsBodyRotation;
                this.PhysicsBodyRotation = 0;

                if (!this.PhysicsBody.IsStatic)
                {
                    if (this.scaleChanged)
                    {
                        if (this.PhysicsVertices != null)
                        {
                            IEnumerable<Vector2> vertices = from vertex in this.PhysicsVertices
                                                            let scaledVertex =
                                                                Vector3.Transform(
                                                                    new Vector3(0, vertex.Y, vertex.X), this.ScaleMatrix)
                                                            select new Vector2(scaledVertex.Z, scaledVertex.Y);
                            this.physicsVertices = new Vertices(vertices.ToArray());
                        }

                        float rotation = this.PhysicsBody.Rotation;
                        this.CreatePhysics();
                        this.offset = new Vector2(this.offset.X * this.scale.X, this.offset.Y * this.scale.Y);
                        this.PhysicsBody.Position = this.Position2D + this.offset;
                        this.PhysicsBody.Rotation = rotation;
                        this.scaleChanged = false;
                    }

                    if (this.AddOffset)
                    {
                        this.Position2D = this.PhysicsBody.Position - this.offset;
                    }
                    else
                    {
                        this.Position2D = this.PhysicsBody.Position;
                    }
                }

                this.Translation = Matrix.CreateTranslation(this.Position3D);
                Matrix translateOffset = Matrix.CreateTranslation(this.offset.X, this.offset.Y, 0);
                Matrix translateOffsetBack = Matrix.CreateTranslation(-this.offset.X, -this.offset.Y, 0);
                this.Rotation = Matrix.CreateFromYawPitchRoll(this.YawRotation, this.PitchRotation, this.RollRotation) *
                                translateOffsetBack * this.PhysicsBody.GetBodyRotationMatrix() * translateOffset;
            }

            this.World = this.ScaleMatrix * this.Rotation * this.Translation;
            this.isUpdating = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The load common content.
        /// </summary>
        protected void LoadCommonContent()
        {
            this.CreatePhysics();

            this.offset = this.PhysicsBody.Position;
            this.PhysicsBody.Position = this.Position2D + this.offset;

            foreach (Effect effect in this.ObjectModel.Meshes.SelectMany(mesh => mesh.Effects))
            {
                if (effect.Parameters["Texture"] != null)
                {
                    this.ModelTextures.Add(effect.Parameters["Texture"].GetValueTexture2D());
                }
                else if (effect.Parameters["BasicTexture"] != null)
                {
                    this.ModelTextures.Add(effect.Parameters["BasicTexture"].GetValueTexture2D());
                }
            }

            base.LoadContent();
        }

        /// <summary>
        /// The load content.
        /// </summary>
        protected override void LoadContent()
        {
            this.ObjectModel = this.Game.Content.Load<Model>(this.modelName);

            ShaderManager.AddEffect(ShaderManager.EFFECT_ID.STATICMODEL, "Effects\\StaticModel", this.Game);
            this.DefaultEffectID = ShaderManager.EFFECT_ID.STATICMODEL;
            this.DefaultTechnique = "StaticModelTechnique";

            if (this.GeometryType == GeometryType.Polygon)
            {
                this.PhysicsVertices = ModelToVertices.TransformStaticModel(this, this.Game, out this.mass);
                this.mass *= 100.0f;
            }

            this.LoadCommonContent();
        }

        /// <summary>
        /// The create physics.
        /// </summary>
        private void CreatePhysics()
        {
            if (this.gridCellSize == -1.0f)
            {
                this.gridCellSize = 0.0f;
            }

            if (this.PhysicsBody != null)
            {
                this.scene.PhysicsSimulator.Remove(this.PhysicsBody);
            }

            if (this.PhysicsGeometry != null)
            {
                this.scene.PhysicsSimulator.Remove(this.PhysicsGeometry);
            }

            if (this.GeometryType == GeometryType.Polygon)
            {
                this.PhysicsBody = BodyFactory.Instance.CreatePolygonBody(
                    this.scene.PhysicsSimulator, this.PhysicsVertices, this.mass * this.scale.X * this.scale.Y);

                this.PhysicsGeometry = GeomFactory.Instance.CreatePolygonGeom(
                    this.scene.PhysicsSimulator, this.PhysicsBody, this.PhysicsVertices, this.gridCellSize);
            }
            else if (this.GeometryType == GeometryType.Circle)
            {
                this.mass =
                    (float)
                    (Math.PI * Math.Pow(this.ObjectModel.Meshes[0].BoundingSphere.Radius * this.scale.Length(), 2));
                this.PhysicsBody = BodyFactory.Instance.CreateCircleBody(
                    this.scene.PhysicsSimulator, 
                    this.ObjectModel.Meshes[0].BoundingSphere.Radius * this.scale.Length(), 
                    this.mass);

                this.PhysicsGeometry = GeomFactory.Instance.CreateCircleGeom(
                    this.scene.PhysicsSimulator, 
                    this.PhysicsBody, 
                    this.ObjectModel.Meshes[0].BoundingSphere.Radius * this.scale.Length(), 
                    100, 
                    this.gridCellSize);
            }
            else if (this.GeometryType == GeometryType.Rectangle)
            {
                BoundingBox box = BoundingBox.CreateFromPoints(GetModelVertices(this.ObjectModel));
                this.mass = (box.Max.X - box.Min.X) * (box.Max.Y - box.Min.Y) * this.scale.X * this.scale.Y;
                this.PhysicsBody = BodyFactory.Instance.CreateRectangleBody(
                    this.scene.PhysicsSimulator, 
                    (box.Max.X - box.Min.X) * this.scale.X, 
                    (box.Max.Y - box.Min.Y) * this.scale.Y, 
                    this.mass);

                this.PhysicsGeometry = GeomFactory.Instance.CreateRectangleGeom(
                    this.scene.PhysicsSimulator, 
                    this.PhysicsBody, 
                    (box.Max.X - box.Min.X) * this.scale.X, 
                    (box.Max.Y - box.Min.Y) * this.scale.Y, 
                    this.gridCellSize);
            }

            this.aabb = this.PhysicsGeometry.AABB;
        }

        #endregion

        /// <summary>
        /// The my struct.
        /// </summary>
        protected struct MyStruct
        {
            #region Constants and Fields

            /// <summary>
            /// The position.
            /// </summary>
            public Vector3 Position;

            /// <summary>
            /// The normal.
            /// </summary>
            public Vector3 RNormal;

            /// <summary>
            /// The texture uv.
            /// </summary>
            public Vector2 TextureUV;

            #endregion
        }
    }
}