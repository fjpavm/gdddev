using Gdd.Game.Engine.Levels.Characters;
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
    /// The Hero health bar
    /// </summary>
    public class HeroHealthBar: DrawableSceneComponent
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

        private Vector2 healthPosition;
        private Texture2D healthTexture;
        private Color[] textureData;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HeroHealthBar"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public HeroHealthBar(Game game)
            : base(game)
        {
            this.healthPosition = new Vector2(0.0f, this.Game.GraphicsDevice.Viewport.Height - 30.0f);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeroHealthBar"/> class.
        /// </summary>
        protected HeroHealthBar()
            : this(null)
        {
        }

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
            this.GraphicsDevice.RenderState.DepthBufferEnable = false;
            this.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;

            this.batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Deferred, SaveStateMode.SaveState);

            this.batch.DrawString(font, "Health", healthPosition, Color.Black);
            this.batch.Draw(healthTexture, new Rectangle((int)healthPosition.X + 80, (int)healthPosition.Y, (int)((this.Game.GraphicsDevice.Viewport.Width - 160) * Hero.GetHeroLife()), 20), new Rectangle((int)healthPosition.X + 80, (int)healthPosition.Y, (int)(this.Game.GraphicsDevice.Viewport.Width * Hero.GetHeroLife()), 20), Color.White);

            this.batch.End();
            this.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
            this.GraphicsDevice.RenderState.DepthBufferEnable = true;
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
            
            textureData = new Color[this.Game.GraphicsDevice.Viewport.Width * 20];

            for (int i = 0; i < this.Game.GraphicsDevice.Viewport.Width; i++)
            {
                float location = (float)i / (float)this.Game.GraphicsDevice.Viewport.Width;
                for (int j = 0; j < 20; j++)
                {
                    textureData[i + this.Game.GraphicsDevice.Viewport.Width * j] = new Color(Color.Green.ToVector3() * MathHelper.Max((0.3f - location) / -0.3f, 0.0f) +
                        Color.Blue.ToVector3() * MathHelper.Max(MathHelper.Min((location - 1.0f) / -0.5f, -location / -0.5f), 0.0f) +
                        Color.Red.ToVector3() * MathHelper.Max((location - 0.7f) / -0.7f, 0.0f));
                    
                }
            }

            healthTexture = new Texture2D(this.Game.GraphicsDevice, this.Game.GraphicsDevice.Viewport.Width, 20);

            healthTexture.SetData<Color>(textureData);
            
            base.LoadContent();
        }

        #endregion
    }
}