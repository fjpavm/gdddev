using FarseerGames.FarseerPhysics.Factories;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PointLight.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   This is a game component that extends Light
// </summary>
// --------------------------------------------------------------------------------------------------------------------

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PointLight.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   This is a game component that extends Light
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Scenes.Lights
{
    using FarseerGames.FarseerPhysics.Collisions;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// This is a game component that extends Light
    /// </summary>
    public class PointLight : Light
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PointLight"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public PointLight(Game game)
            : base(game)
        {
            this.pos3D.Z = -1.0f;
            this.Color = this.Color;
            this.Radius = 10.0f;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The has changed.
        /// </summary>
        private bool hasChanged;

        /// <summary>
        /// Gets or sets Radius.
        /// </summary>
        private float radius;

        public float Radius
        {
            get
            {
                return this.radius;
            }

            set
            {
                this.radius= value;
                this.hasChanged = true;
            }
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// The initialize.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (this.hasChanged)
            {
                this.LoadContent();
                this.hasChanged = false;
            }

            base.Update(gameTime);
            this.PhysicsBody.Position = this.pos2D;
            this.aabb = this.PhysicsGeometry.AABB;
        }

        #endregion

        protected override void LoadContent()
        {
            base.LoadContent();

            this.PhysicsBody = BodyFactory.Instance.CreateRectangleBody(2 * this.Radius, 2 * this.Radius, 1.0f);
            this.PhysicsGeometry = new Physics.GeomDC(this,GeomFactory.Instance.CreateRectangleGeom(this.PhysicsBody, 2 * this.Radius, 2 * this.Radius));
            this.aabb = this.PhysicsGeometry.AABB;
        }
    }
}