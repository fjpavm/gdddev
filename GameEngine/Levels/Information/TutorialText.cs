// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TutorialText.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The tutorial text.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels.Information
{
    using System;

    using Gdd.Game.Engine.Render;
    using Gdd.Game.Engine.Render.Shadow;
    using Gdd.Game.Engine.Scenes;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The tutorial text.
    /// </summary>
    public class TutorialText : DrawableSceneComponent
    {
        #region Constants and Fields

        /// <summary>
        /// The body font.
        /// </summary>
        private static SpriteFont BodyFont;

        /// <summary>
        /// The header font.
        /// </summary>
        private static SpriteFont HeaderFont;

        /// <summary>
        /// The texture sprite vertex declaration.
        /// </summary>
        private static VertexDeclaration TextureSpriteVertexDeclaration;

        /// <summary>
        /// The text sprite.
        /// </summary>
        private TextureSprite[] textSprite;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TutorialText"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public TutorialText(Game game)
            : base(game)
        {
            if (TextureSpriteVertexDeclaration == null)
            {
                TextureSpriteVertexDeclaration = new VertexDeclaration(
                    game.GraphicsDevice, TextureSprite.VertexElements);
            }

            if (HeaderFont == null)
            {
                throw new ArgumentNullException("Font has not been set, call TutorialText.SetFonts before drawing");
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets BodyText.
        /// </summary>
        public string BodyText { get; set; }

        /// <summary>
        /// Gets or sets HeaderText.
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// Gets or sets TextBoxSize.
        /// </summary>
        public Vector2 TextBoxSize { get; set; }

        /// <summary>
        /// Gets or sets DialogTexture.
        /// </summary>
        private Texture2D DialogTexture { get; set; }

        /// <summary>
        /// Gets or sets TextTexture.
        /// </summary>
        private Texture2D TextTexture { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The set fonts.
        /// </summary>
        /// <param name="headerFont">
        /// The header font.
        /// </param>
        /// <param name="bodyFont">
        /// The body font.
        /// </param>
        public static void SetFonts(SpriteFont headerFont, SpriteFont bodyFont)
        {
            HeaderFont = headerFont;
            BodyFont = bodyFont;
        }

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public override void Draw(GameTime gameTime)
        {
            if (this.HeaderText == null)
            {
                throw new ArgumentNullException("HeaderText should not be null");
            }

            if (this.BodyText == null)
            {
                throw new ArgumentNullException("BodyText should not be null");
            }

            if (this.TextBoxSize == null)
            {
                throw new ArgumentNullException("TextBoxSize should not be null");
            }

            // draw the texture
            ShaderManager.SetCurrentEffect(ShaderManager.EFFECT_ID.TEXTURE);

            Matrix worldMatrix = Matrix.Identity;
            ShaderManager.SetValue("World", Matrix.CreateTranslation(this.Position3D));
            ShaderManager.SetValue("View", this.scene.Camera.View);
            ShaderManager.SetValue("Projection", this.scene.Camera.Perspective);

            ShaderManager.SetValue("tex", this.TextTexture);

            ShaderManager.CommitChanges();

            ShaderManager.Begin();
            ShaderManager.GetCurrentEffect().Techniques["TextureTechnique"].Passes[0].Begin();

            this.GraphicsDevice.VertexDeclaration = TextureSpriteVertexDeclaration;
            this.Game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, this.textSprite, 0, 2);

            ShaderManager.GetCurrentEffect().Techniques["TextureTechnique"].Passes[0].End();

            ShaderManager.End();
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
            this.DialogTexture = this.Game.Content.Load<Texture2D>("Textures\\DialogTexture");

            ShaderManager.AddEffect(ShaderManager.EFFECT_ID.TEXTURE, "TextureEffect", this.Game);

            // create the texture that will be shown
            var renderTarget = new RenderTarget2D(
                this.Game.GraphicsDevice, 
                512, 
                (int)(512 * (this.TextBoxSize.Y / this.TextBoxSize.X)), 
                1, 
                SurfaceFormat.Color);
            var depthBuffer = new DepthStencilBuffer(
                this.Game.GraphicsDevice, 
                512, 
                (int)(512 * (this.TextBoxSize.Y / this.TextBoxSize.X)), 
                DepthFormat.Depth16);

            DepthStencilBuffer old = ShadowMapManager.SetupShadowMap(
                this.Game.GraphicsDevice, ref renderTarget, ref depthBuffer, Color.TransparentWhite);

            var spriteBatch = (SpriteBatch)this.scene.Game.Services.GetService(typeof(SpriteBatch));

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

            spriteBatch.Draw(
                this.DialogTexture, 
                new Rectangle(0, 0, 512, (int)(512 * (this.TextBoxSize.Y / this.TextBoxSize.X))), 
                Color.White);

            spriteBatch.DrawString(
                HeaderFont, 
                this.HeaderText, 
                new Vector2(renderTarget.Width / 2.0f, renderTarget.Height / 10.0f + 30), 
                Color.Black, 
                0.0f, 
                HeaderFont.MeasureString(this.HeaderText) / 2.0f, 
                1.0f, 
                SpriteEffects.None, 
                1.0f);
            spriteBatch.DrawString(
                BodyFont, 
                this.BodyText, 
                new Vector2(renderTarget.Width / 2.0f, renderTarget.Height / 10.0f) +
                new Vector2(0.0f, 2 * HeaderFont.MeasureString(this.HeaderText).Y + 30), 
                Color.Black, 
                0.0f, 
                BodyFont.MeasureString(this.BodyText) / 2.0f, 
                1.0f, 
                SpriteEffects.None, 
                1.0f);

            spriteBatch.End();

            ShadowMapManager.ResetGraphicsDevice(this.Game.GraphicsDevice, old);
            this.TextTexture = renderTarget.GetTexture();

            Vector3 HalfTextBoxSizeWidth, HalfTextBoxSizeHeight;
            HalfTextBoxSizeHeight = new Vector3(0.0f, this.TextBoxSize.Y / 2.0f, 0.0f);
            HalfTextBoxSizeWidth = new Vector3(this.TextBoxSize.X / 2.0f, 0.0f, 0.0f);

            this.textSprite = new[]
                {
                    new TextureSprite(
                        this.Position3D - HalfTextBoxSizeWidth - HalfTextBoxSizeHeight, new Vector2(0.0f, 1.0f)), 
                    new TextureSprite(this.Position3D - HalfTextBoxSizeWidth + HalfTextBoxSizeHeight, Vector2.Zero), 
                    new TextureSprite(
                        this.Position3D + HalfTextBoxSizeWidth - HalfTextBoxSizeHeight, new Vector2(1.0f, 1.0f)), 
                    new TextureSprite(
                        this.Position3D + HalfTextBoxSizeWidth + HalfTextBoxSizeHeight, new Vector2(1.0f, 0.0f)), 
                };
        }

        #endregion

        /// <summary>
        /// The texture sprite.
        /// </summary>
        private struct TextureSprite
        {
            #region Constants and Fields

            /// <summary>
            /// The vertex elements.
            /// </summary>
            public static readonly VertexElement[] VertexElements = {
                                                                        new VertexElement(
                                                                            0, 
                                                                            0, 
                                                                            VertexElementFormat.Vector3, 
                                                                            VertexElementMethod.Default, 
                                                                            VertexElementUsage.Position, 
                                                                            0), 
                                                                        new VertexElement(
                                                                            0, 
                                                                            sizeof(float) * 3, 
                                                                            VertexElementFormat.Vector2, 
                                                                            VertexElementMethod.Default, 
                                                                            VertexElementUsage.TextureCoordinate, 
                                                                            0), 
                                                                    };

            /// <summary>
            /// The size in bytes.
            /// </summary>
            public static int SizeInBytes = sizeof(float) * (3 + 2);

            /// <summary>
            /// The uv.
            /// </summary>
            private Vector2 UV;

            /// <summary>
            /// The position.
            /// </summary>
            private Vector3 position;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="TextureSprite"/> struct.
            /// </summary>
            /// <param name="position">
            /// The position.
            /// </param>
            /// <param name="UV">
            /// The uv.
            /// </param>
            public TextureSprite(Vector3 position, Vector2 UV)
            {
                this.position = position;
                this.UV = UV;
            }

            #endregion
        }
    }
}