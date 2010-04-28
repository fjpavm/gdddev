using System.Collections.Generic;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaticModel.cs" company="GDD">
//   Game Design and Development
// </copyright>
// <summary>
//   The static model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using System.Reflection;
    using System.Xml.Serialization;

    using FarseerGames.FarseerPhysics.Collisions;
    using FarseerGames.FarseerPhysics.Dynamics;
    using FarseerGames.FarseerPhysics.Factories;

    using Characters;

    using Physics;

    using Render;

    using Scenes;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using System.Runtime.InteropServices;
    using System.Linq;

    /// <summary>
    /// The static model.
    /// </summary>
    public class StaticModel : DrawableSceneComponent
    {
        public enum DIRECTION { LEFT, RIGHT };
        public enum GEOMETRY_TYPE { POLYGON, CIRCLE, RECTANGLE };

        #region Constants and Fields

        /// <summary>
        /// The model name.
        /// </summary>
        protected string modelName;

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
            this.YawRotation = MathHelper.PiOver2;
            this.PitchRotation = 0.0f;
            this.RollRotation = 0.0f;

            this.Rotation = Matrix.CreateFromYawPitchRoll(YawRotation, PitchRotation, RollRotation);

            this.Direction = DIRECTION.RIGHT;;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticModel"/> class.
        /// </summary>
        protected StaticModel()
            : this(null)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// The geometry type of the object
        /// </summary>
        public GEOMETRY_TYPE geoType { get; set; }

        protected float gridCellSize = -1.0f;

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
        [XmlIgnore]
        public Model ObjectModel { get; protected set; }

        /// <summary>
        /// Gets or sets PhysicsTexture.
        /// </summary>
        [XmlIgnore]
        public Texture2D PhysicsTexture { get; protected set; }

        /// <summary>
        /// Gets or sets PhysicsVertices.
        /// </summary>
        [XmlIgnore]
        public Vertices PhysicsVertices { get; protected set; }

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
                if (this.PhysicsBody != null)
                {
                    this.PhysicsBody.Position = value + this.offset;
                }
            }
        }

        /// <summary>
        /// Gets or sets ModelTexture.
        /// </summary>
        [XmlIgnore]
        private List<Texture2D> textures;
        [XmlIgnore]
        protected List<Texture2D> ModelTextures {
            get
            {
                if (textures == null)
                    textures = new List<Texture2D>();
                return textures;
            }
            set{
                textures = value;
            } 
        }

        /// <summary>
        /// The rotation along the X axis
        /// </summary>
        [XmlIgnore]
        protected float yawRotation, pitchRotation, rollRotation;

        [XmlIgnore]
        public float YawRotation{
            get { return yawRotation; }
            set{
                yawRotation = value;
                Rotation = Matrix.CreateFromYawPitchRoll(yawRotation, pitchRotation, rollRotation);
            }
        }

        [XmlIgnore]
        public float PitchRotation
        {
            get { return pitchRotation; }
            set
            {
                pitchRotation = value;
                Rotation = Matrix.CreateFromYawPitchRoll(yawRotation, pitchRotation, rollRotation);
            }
        }

        [XmlIgnore]
        public float RollRotation
        {
            get { return rollRotation; }
            set
            {
                rollRotation = value;
                Rotation = Matrix.CreateFromYawPitchRoll(yawRotation, pitchRotation, rollRotation);
            }
        }

        [XmlIgnore]
        public DIRECTION Direction { get; protected set; }

        #endregion

        #region Public Methods

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
                        ShaderManager.SetValue("Texture", this.ModelTextures[count++]);

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
        /// The initialize.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// The set texture.
        /// </summary>
        /// <param name="texture">
        /// The texture.
        /// </param>
        public void SetTexture(Texture2D texture)
        {
            ModelTextures.Add(texture);
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Update(GameTime gameTime)
        {
            if (this.PhysicsBody != null && !this.PhysicsBody.IsStatic)
            {
                Position2D = PhysicsBody.Position - offset;
                
                this.Translation = Matrix.CreateTranslation(Position3D);

                this.Rotation = Matrix.CreateFromYawPitchRoll(YawRotation, PitchRotation, RollRotation) * PhysicsBody.GetBodyRotationMatrix();

                this.World = Matrix.CreateTranslation(new Vector3(-this.PhysicsGeometry.LocalVertices.GetCentroid(), 0.0f)) * this.Rotation * this.Translation;
            }
            else
            {
                this.World = this.Rotation * this.Translation;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The load content.
        /// </summary>
        protected override void LoadContent()
        {
            this.ObjectModel = this.Game.Content.Load<Model>(this.modelName);

            ShaderManager.AddEffect(ShaderManager.EFFECT_ID.STATICMODEL, "Effects\\StaticModel", this.Game);
            this.DefaultEffectID = ShaderManager.EFFECT_ID.STATICMODEL;
            this.DefaultTechnique = "StaticModelTechnique";
            
            if (geoType == GEOMETRY_TYPE.POLYGON)
            {
                this.PhysicsVertices = ModelToVertices.TransformStaticModel(this, this.Game);
            }

            LoadCommonContent();

            this.PhysicsBody.IsStatic = true;
        }

        protected void LoadCommonContent(){
            if (gridCellSize == -1.0f)
                gridCellSize = 0.0f;

            if (geoType == GEOMETRY_TYPE.POLYGON)
            {
                this.PhysicsBody = BodyFactory.Instance.CreatePolygonBody(
                    SceneManager.physicsSimulator, this.PhysicsVertices, 10.0f);

                this.PhysicsGeometry = GeomFactory.Instance.CreatePolygonGeom(
                    SceneManager.physicsSimulator, this.PhysicsBody, PhysicsVertices, gridCellSize);
            }
            else if(geoType == GEOMETRY_TYPE.CIRCLE){
                this.PhysicsBody = BodyFactory.Instance.CreateCircleBody(SceneManager.physicsSimulator,this.ObjectModel.Meshes[0].BoundingSphere.Radius, 10.0f);

                this.PhysicsGeometry = GeomFactory.Instance.CreateCircleGeom(
                    SceneManager.physicsSimulator, this.PhysicsBody, this.ObjectModel.Meshes[0].BoundingSphere.Radius, 20, gridCellSize);
            }
            else if (geoType == GEOMETRY_TYPE.RECTANGLE)
            {
                BoundingBox box = BoundingBox.CreateFromPoints(GetModelVertices(this.ObjectModel));
                this.PhysicsBody = BodyFactory.Instance.CreateRectangleBody(SceneManager.physicsSimulator, box.Max.X - box.Min.X, box.Max.Y - box.Min.Y , 10.0f);
                
                this.PhysicsGeometry = GeomFactory.Instance.CreateRectangleGeom(
                    SceneManager.physicsSimulator, this.PhysicsBody, box.Max.X - box.Min.X, box.Max.Y - box.Min.Y, gridCellSize);
            }

            this.offset = this.PhysicsBody.Position;
            this.PhysicsBody.Position = this.Position2D + this.offset;

            this.aabb = this.PhysicsGeometry.AABB;

            //FieldInfo fi = typeof(Body).GetField("_previousPosition", BindingFlags.Instance | BindingFlags.NonPublic);
            //fi.SetValue(this.PhysicsBody, this.PhysicsBody.Position);

            foreach (ModelMesh mesh in this.ObjectModel.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    if (effect.Parameters["Texture"] != null)
                        this.ModelTextures.Add(effect.Parameters["Texture"].GetValueTexture2D());
                    else if (effect.Parameters["BasicTexture"] != null)
                        this.ModelTextures.Add(effect.Parameters["BasicTexture"].GetValueTexture2D());
                }
            }

            base.LoadContent();
        }

        #endregion

        public Vector2 offset;

        #region Static model methods

        public static int GetSizeOfMesh(Model objectModel)
        {
            int sizeofVertex = 0;
            foreach (ModelMesh modelMesh in objectModel.Meshes)
            {
                foreach (ModelMeshPart modelMeshPart in modelMesh.MeshParts)
                {
                    VertexElement[] vertexElements = modelMeshPart.VertexDeclaration.GetVertexElements();
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

        public static Vector3[] GetModelVertices(Model objectModel){
            int sizeOfVertex = GetSizeOfMesh(objectModel);

            List<Vector3> vertices = new List<Vector3>();
            
            foreach(ModelMesh mesh in objectModel.Meshes){
                var meshBuffer = new MyStruct[mesh.VertexBuffer.SizeInBytes / sizeOfVertex];

                mesh.VertexBuffer.GetData<MyStruct>(meshBuffer);
                var vertexBuffer = from v in meshBuffer select new Vector3(v.Position.Z, v.Position.Y, v.Position.X);
       
                vertices.AddRange(vertexBuffer);       
            }

            return vertices.ToArray();
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
            public Vector3 Normal;


            /// <summary>
            /// The texture uv.
            /// </summary>
            public Vector2 TextureUV;

            #endregion
        }
    }


    
}