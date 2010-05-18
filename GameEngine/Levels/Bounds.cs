// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Bounds.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The bounds.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using FarseerGames.FarseerPhysics.Factories;

    using Gdd.Game.Engine.Levels.Characters;
    using Gdd.Game.Engine.Scenes;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// The bounds.
    /// </summary>
    public class Bounds : DrawableSceneComponent
    {
        #region Constants and Fields

        /// <summary>
        /// The has changed.
        /// </summary>
        private bool hasChanged;

        /// <summary>
        /// The size.
        /// </summary>
        private Vector2 size;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Bounds"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public Bounds(Game game)
            : base(game)
        {
            this.Size = new Vector2(10, 10);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether IsColliding.
        /// </summary>
        public bool IsColliding { get; private set; }

        /// <summary>
        /// Gets or sets Size.
        /// </summary>
        public Vector2 Size
        {
            get
            {
                return this.size;
            }

            set
            {
                this.size = value;
                this.hasChanged = true;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
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

            if (Hero.HeroGeometry != null)
            {
                this.IsColliding = this.PhysicsGeometry.Collide(Hero.HeroGeometry);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The load content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            this.PhysicsBody = BodyFactory.Instance.CreateRectangleBody(this.Size.X, this.Size.Y, 1.0f);
            this.PhysicsGeometry = new Physics.GeomDC(this,GeomFactory.Instance.CreateRectangleGeom(this.PhysicsBody, this.Size.X, this.Size.Y));
            this.aabb = this.PhysicsGeometry.AABB;
        }

        #endregion
    }
}