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

    using FarseerGames.FarseerPhysics.Factories;

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

            this.HeaderText = string.Empty;
            this.BodyText = string.Empty;
            this.TextBoxSize = new Vector2(10);
        }

        #endregion

        #region Properties

        private SpriteFont bodyFont;

        private bool hasChanged;

        /// <summary>
        /// Gets or sets BodyFont.
        /// </summary>
        public SpriteFont BodyFont
        {
            get
            {
                return this.bodyFont;
            }
            set
            {
                this.bodyFont = value;
                this.hasChanged = true;
            }
        }

        private string bodyText;

        /// <summary>
        /// Gets or sets BodyText.
        /// </summary>
        public string BodyText
        {
            get
            {
                return this.bodyText;
            }
            set
            {
                this.bodyText = value;
                this.hasChanged = true;
            }
        }

        private SpriteFont headerFont;

        /// <summary>
        /// The header font.
        /// </summary>
        public SpriteFont HeaderFont
        {
            get
            {
                return this.headerFont;
            }
            set
            {
                this.headerFont = value;
                this.hasChanged = true;
            }
        }

        private string headerText;

        /// <summary>
        /// Gets or sets HeaderText.
        /// </summary>
        public string HeaderText
        {
            get
            {
                return this.headerText;
            }
            set
            {
                this.headerText = value;
                this.hasChanged = true;
            }
        }

        private Vector2 textBoxSize;

        /// <summary>
        /// Gets or sets TextBoxSize.
        /// </summary>
        public Vector2 TextBoxSize
        {
            get
            {
                return this.textBoxSize;
            }
            set
            {
                this.textBoxSize = value;
                this.hasChanged = true;
            }
        }

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

            ShaderManager.SetValue("World", Matrix.CreateTranslation(this.Position3D));
            ShaderManager.SetValue("View", this.scene.Camera.View);
            ShaderManager.SetValue("Projection", this.scene.Camera.Perspective);

            ShaderManager.SetValue("tex", this.TextTexture);

            ShaderManager.CommitChanges();

            ShaderManager.Begin();
            ShaderManager.GetCurrentEffect().Techniques["TextureTechnique"].Passes[0].Begin();

            this.Game.GraphicsDevice.VertexDeclaration = TextureSpriteVertexDeclaration;
            this.Game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, this.textSprite, 0, 2);

            ShaderManager.GetCurrentEffect().Techniques["TextureTechnique"].Passes[0].End();

            ShaderManager.End();
            this.DrawPhysicsVertices();
        }

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
        }

        #endregion

        #region Methods

        /// <summary>
        /// The load content.
        /// </summary>
        protected override void LoadContent()
        {
            this.HeaderFont = this.Game.Content.Load<SpriteFont>("Font//TutorialHeader");
            this.BodyFont = this.Game.Content.Load<SpriteFont>("Font//TutorialBody");
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

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.SaveState);

            spriteBatch.Draw(
                this.DialogTexture, 
                new Rectangle(0, 0, 512, (int)(512 * (this.TextBoxSize.Y / this.TextBoxSize.X))), 
                Color.White);

            spriteBatch.DrawString(
                this.HeaderFont, 
                this.HeaderText, 
                new Vector2(renderTarget.Width / 2.0f, renderTarget.Height / 10.0f + 30), 
                Color.Black, 
                0.0f, 
                this.HeaderFont.MeasureString(this.HeaderText) / 2.0f, 
                1.0f, 
                SpriteEffects.None, 
                1.0f);
            spriteBatch.DrawString(
                this.BodyFont, 
                this.BodyText, 
                new Vector2(renderTarget.Width / 2.0f, renderTarget.Height / 10.0f) +
                new Vector2(0.0f, 2 * this.HeaderFont.MeasureString(this.HeaderText).Y + 30), 
                Color.Black, 
                0.0f, 
                this.BodyFont.MeasureString(this.BodyText) / 2.0f, 
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
                        - HalfTextBoxSizeWidth - HalfTextBoxSizeHeight, new Vector2(0.0f, 1.0f)), 
                    new TextureSprite(- HalfTextBoxSizeWidth + HalfTextBoxSizeHeight, Vector2.Zero), 
                    new TextureSprite(
                        HalfTextBoxSizeWidth - HalfTextBoxSizeHeight, new Vector2(1.0f, 1.0f)), 
                    new TextureSprite(
                        HalfTextBoxSizeWidth + HalfTextBoxSizeHeight, new Vector2(1.0f, 0.0f)), 
                };

            this.PhysicsBody = BodyFactory.Instance.CreateRectangleBody(HalfTextBoxSizeWidth.X * 2, HalfTextBoxSizeHeight.Y * 2, 1.0f);
            this.PhysicsGeometry = GeomFactory.Instance.CreateRectangleGeom(this.PhysicsBody, HalfTextBoxSizeWidth.X * 2, HalfTextBoxSizeHeight.Y * 2);
            this.aabb = this.PhysicsGeometry.AABB;
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
            /// The position.
            /// </summary>
            private Vector3 position;

            /// <summary>
            /// The uv.
            /// </summary>
            private Vector2 UV;


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