// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeroHealthBar.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The Hero health bar
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels.Characters
{
    using Gdd.Game.Engine.Scenes;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The Hero health bar
    /// </summary>
    public class HeroHealthBar : DrawableSceneComponent
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

        /// <summary>
        /// The health position.
        /// </summary>
        private Vector2 healthPosition;

        /// <summary>
        /// The health texture.
        /// </summary>
        private Texture2D healthTexture;

        /// <summary>
        /// The texture data.
        /// </summary>
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
            /*this.GraphicsDevice.RenderState.DepthBufferEnable = false;
            this.GraphicsDevice.RenderState.DepthBufferWriteEnable = false;*/

            this.batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

            this.batch.DrawString(this.font, "Health", this.healthPosition, Color.Black);
            this.batch.Draw(
                this.healthTexture, 
                new Rectangle(
                    (int)this.healthPosition.X + 80, 
                    (int)this.healthPosition.Y, 
                    (int)((this.Game.GraphicsDevice.Viewport.Width - 160) * Hero.GetHeroLife()), 
                    20), 
                new Rectangle(
                    (int)this.healthPosition.X + 80, 
                    (int)this.healthPosition.Y, 
                    (int)(this.Game.GraphicsDevice.Viewport.Width * Hero.GetHeroLife()), 
                    20), 
                Color.White);

            this.batch.End();
          /*  this.GraphicsDevice.RenderState.DepthBufferWriteEnable = true;
            this.GraphicsDevice.RenderState.DepthBufferEnable = true;*/
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

            this.textureData = new Color[this.Game.GraphicsDevice.Viewport.Width * 20];

            for (int i = 0; i < this.Game.GraphicsDevice.Viewport.Width; i++)
            {
                float location = i / (float)this.Game.GraphicsDevice.Viewport.Width;
                for (int j = 0; j < 20; j++)
                {
                    this.textureData[i + this.Game.GraphicsDevice.Viewport.Width * j] =
                        new Color(
                            Color.Green.ToVector3() * MathHelper.Max((0.3f - location) / -0.3f, 0.0f) +
                            Color.Blue.ToVector3() *
                            MathHelper.Max(MathHelper.Min((location - 1.0f) / -0.5f, -location / -0.5f), 0.0f) +
                            Color.Red.ToVector3() * MathHelper.Max((location - 0.7f) / -0.7f, 0.0f));
                }
            }

            this.healthTexture = new Texture2D(this.Game.GraphicsDevice, this.Game.GraphicsDevice.Viewport.Width, 20);

            this.healthTexture.SetData(this.textureData);

            base.LoadContent();
        }

        #endregion
    }
}