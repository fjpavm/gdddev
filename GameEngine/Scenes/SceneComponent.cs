using FarseerGames.FarseerPhysics.Collisions;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SceneComponent.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The s component.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Scenes
{
    using System;
    using System.Reflection;
    using System.Xml.Serialization;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// The s component.
    /// </summary>
    public class SceneComponent : GameComponent
    {
        #region Constants and Fields

        /// <summary>
        /// The game field info.
        /// </summary>
        private static readonly FieldInfo GameFieldInfo;

        /// <summary>
        /// Initializes static members of the <see cref="SceneComponent"/> class.
        /// </summary>
        static SceneComponent()
        {
            GameFieldInfo = typeof(GameComponent).GetField("game", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        /// <summary>
        /// The set game.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public void SetGame(Game game)
        {
            GameFieldInfo.SetValue(this, game);
        }

        /// <summary>
        /// The s.
        /// </summary>
        protected Scene scene;

        /// <summary>
        /// The 3D position.
        /// </summary>
        protected Vector3 pos3D;

        protected Vector3 prevPos3D;

        /// <summary>
        /// The 2D position
        /// </summary>
        protected Vector2 pos2D;

        protected Vector2 prevPos2D;

        /// <summary>
        ///  world inverse transpose 
        /// </summary>
        protected Matrix InverseTransposeWorld;

        ///<summary>
        /// Gets and sets the World matrix
        ///</summary>
        [XmlIgnore]
        public Matrix World { get; protected set; }

        ///<summary>
        /// Gets and sets the Translation matrix
        ///</summary>
        [XmlIgnore]
        public Matrix Translation { get; protected set; }

        ///<summary>
        /// Gets and sets the Rotation matrix
        ///</summary>
        [XmlIgnore]
        public Matrix Rotation { get; protected set; }

        /// <summary>
        /// The is changed.
        /// </summary>
        protected bool isChanged;

        /// <summary>
        /// The axis aligned bounding box.
        /// </summary>
        [XmlIgnore]
        public AABB aabb { get; set; }

        /// <summary>
        /// The Position in 2D, setting it will change the Position in 3D as well
        /// </summary>
        public virtual Vector2 Position2D
        {
            get
            {
                return this.pos2D;
            }

            set
            {
                this.prevPos2D = this.pos2D;
                this.pos2D = value;
                this.prevPos3D = this.pos3D;
                this.pos3D.X = this.pos2D.X;
                this.pos3D.Y = this.pos2D.Y;
                this.pos3D.Z = SceneManager.Z_POSITION;
                this.Translation = Matrix.CreateTranslation(this.pos3D);

                InverseTransposeWorld = Matrix.Invert(Matrix.Transpose(Translation));

                this.isChanged = true;
            }
        }

        /// <summary>
        /// Gets Position3D.
        /// </summary>
        [XmlIgnore]
        public virtual Vector3 Position3D
        {
            get
            {
                return this.pos3D;
            }

            set
            {
                throw new MethodAccessException();
            }
        }

        public Vector2 PrevPos2D
        {
            get
            {
                return this.prevPos2D;
            }
        }

        public Vector3 PrevPos3D
        {
            get
            {
                return this.prevPos3D;
            }
        }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneComponent"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public SceneComponent(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneComponent"/> class.
        /// </summary>
        protected SceneComponent()
            : base(null)
        {
        }

        #endregion

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        #region Public Methods

        /// <summary>
        /// The set s.
        /// </summary>
        /// <param name="s">
        /// The Scene instance.
        /// </param>
        public void SetScene(Scene s)
        {
            this.scene = s;
        }

        #endregion
    }
}