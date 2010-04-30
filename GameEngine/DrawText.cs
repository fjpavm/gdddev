// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawText.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The draw text.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine
{
    using Scenes;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The draw text.
    /// </summary>
    public class DrawText : DrawableSceneComponent
    {
        #region Constants and Fields

        /// <summary>
        /// The batch.
        /// </summary>
        private SpriteBatch batch;

        /// <summary>
        /// The font.
        /// </summary>
        private SpriteFont font;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawText"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public DrawText(Game game)
            : base(game)
        {
            this.Text = "text";
        }

        #endregion

        #region Properties

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
                if (this.scene != null)
                {
                    Vector2 projection = this.scene.Camera.Project(this.Position3D);
                    this.pos2D = new Vector2(projection.X, projection.Y);
                }
            }
        }

        /// <summary>
        /// Gets or sets Text.
        /// </summary>
        public string Text { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Draw(GameTime gameTime)
        {
            this.batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);
            this.batch.DrawString(this.font, this.Text, this.Position2D, Color.Blue);
            this.batch.End();
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        public override void Initialize()
        {
            this.batch = new SpriteBatch(this.Game.GraphicsDevice);

            // calls the LoadContent function
            base.Initialize();
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The load content.
        /// </summary>
        protected override void LoadContent()
        {
            this.font = this.Game.Content.Load<SpriteFont>("font");
            base.LoadContent();
        }

        #endregion
    }
}