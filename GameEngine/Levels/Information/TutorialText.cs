using System;
using Gdd.Game.Engine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using Gdd.Game.Engine.Render.Shadow;
using Gdd.Game.Engine.Render;

namespace Gdd.Game.Engine.Levels.Information
{
    public class TutorialText : DrawableSceneComponent
    {
        [XmlIgnore]
        public String HeaderText {get; set;}
        [XmlIgnore]
        public String BodyText { get; set; }
        [XmlIgnore]
        public Vector2 TextBoxSize { get; set; }
        [XmlIgnore]
        private Texture2D TextTexture { get; set; }
        [XmlIgnore]
        private Texture2D DialogTexture {get; set;}
        [XmlIgnore]
        private TextureSprite[] textSprite;
        
        private static SpriteFont HeaderFont;
        private static SpriteFont BodyFont;
        
        private static VertexDeclaration TextureSpriteVertexDeclaration;        
        
        private struct TextureSprite
        {            
            private Vector3 position;
            private Vector2 UV;

            public TextureSprite(Vector3 position, Vector2 UV)
            {
                this.position = position;
                this.UV = UV;
            }

            public static readonly VertexElement[] VertexElements =
            {
                  new VertexElement(0, 0, VertexElementFormat.Vector3, VertexElementMethod.Default, VertexElementUsage.Position, 0),
                  new VertexElement(0, sizeof(float)*3, VertexElementFormat.Vector2, VertexElementMethod.Default, VertexElementUsage.TextureCoordinate, 0),
            };

            public static int SizeInBytes = sizeof(float) * (3 + 2);
        }

        public static void SetFonts(SpriteFont headerFont, SpriteFont bodyFont){
            HeaderFont = headerFont;
            BodyFont = bodyFont;
        }

        public TutorialText() : base (null){}

        public TutorialText(Microsoft.Xna.Framework.Game game, Vector2 textBoxSize)
            : base(game)
        {
            TextBoxSize = textBoxSize;
            
            if (TextureSpriteVertexDeclaration == null)
                TextureSpriteVertexDeclaration = new VertexDeclaration(game.GraphicsDevice, TextureSprite.VertexElements);

            if (HeaderFont == null)
            {
                throw new ArgumentNullException("Font has not been set, call TutorialText.SetFonts before drawing");
            }
        }

        protected override void LoadContent()
        {
            DialogTexture = this.Game.Content.Load<Texture2D>("Textures\\DialogTexture");

            ShaderManager.AddEffect(ShaderManager.EFFECT_ID.TEXTURE, "TextureEffect", this.Game);

            // create the texture that will be shown
            RenderTarget2D renderTarget = new RenderTarget2D(this.Game.GraphicsDevice, 512, (int)(512 * (TextBoxSize.Y / TextBoxSize.X)), 1, SurfaceFormat.Color);
            DepthStencilBuffer depthBuffer = new DepthStencilBuffer(this.Game.GraphicsDevice, 512, (int)(512* (TextBoxSize.Y / TextBoxSize.X)), DepthFormat.Depth16);

            DepthStencilBuffer old = ShadowMapManager.SetupShadowMap(this.Game.GraphicsDevice, ref renderTarget, ref depthBuffer, Color.TransparentWhite);

            var spriteBatch = (SpriteBatch)this.scene.Game.Services.GetService(typeof(SpriteBatch));

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);
            
            spriteBatch.Draw(DialogTexture, new Rectangle(0, 0, 512, (int)(512 * (TextBoxSize.Y / TextBoxSize.X))), Color.White);
            
            spriteBatch.DrawString(HeaderFont, HeaderText, new Vector2((float)renderTarget.Width / 2.0f, (float)renderTarget.Height / 10.0f + 30), Color.Black, 0.0f, HeaderFont.MeasureString(HeaderText) / 2.0f, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.DrawString(BodyFont, BodyText, new Vector2((float)renderTarget.Width / 2.0f, (float)renderTarget.Height / 10.0f) + new Vector2(0.0f, 2 * HeaderFont.MeasureString(HeaderText).Y + 30), Color.Black, 0.0f, BodyFont.MeasureString(BodyText) / 2.0f, 1.0f, SpriteEffects.None, 1.0f);

            spriteBatch.End();

            ShadowMapManager.ResetGraphicsDevice(this.Game.GraphicsDevice, old);
            TextTexture = renderTarget.GetTexture();

            Vector3 HalfTextBoxSizeWidth, HalfTextBoxSizeHeight;
            HalfTextBoxSizeHeight = new Vector3(0.0f, TextBoxSize.Y / 2.0f, 0.0f);
            HalfTextBoxSizeWidth = new Vector3(TextBoxSize.X / 2.0f, 0.0f, 0.0f);

            textSprite = new TextureSprite[]{
                new TextureSprite(Position3D - HalfTextBoxSizeWidth - HalfTextBoxSizeHeight, new Vector2(0.0f, 1.0f)), 
                new TextureSprite(Position3D - HalfTextBoxSizeWidth + HalfTextBoxSizeHeight, Vector2.Zero), 
                new TextureSprite(Position3D + HalfTextBoxSizeWidth - HalfTextBoxSizeHeight, new Vector2(1.0f, 1.0f)), 
                new TextureSprite(Position3D + HalfTextBoxSizeWidth + HalfTextBoxSizeHeight, new Vector2(1.0f, 0.0f)), 
            };
        }
        
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (HeaderText == null)
                throw new ArgumentNullException("HeaderText should not be null");
            if (BodyText == null)
                throw new ArgumentNullException("BodyText should not be null");
            if (TextBoxSize == null)
                throw new ArgumentNullException("TextBoxSize should not be null");

            // draw the texture
            ShaderManager.SetCurrentEffect(ShaderManager.EFFECT_ID.TEXTURE);

            Matrix worldMatrix = Matrix.Identity;
            ShaderManager.SetValue("World", Matrix.CreateTranslation(Position3D));
            ShaderManager.SetValue("View", this.scene.Camera.View);
            ShaderManager.SetValue("Projection", this.scene.Camera.Perspective);
            
            ShaderManager.SetValue("tex", TextTexture);

            ShaderManager.CommitChanges();

            ShaderManager.Begin();
            ShaderManager.GetCurrentEffect().Techniques["TextureTechnique"].Passes[0].Begin();

            this.GraphicsDevice.VertexDeclaration = TextureSpriteVertexDeclaration;
            this.Game.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, textSprite, 0, 2);

            ShaderManager.GetCurrentEffect().Techniques["TextureTechnique"].Passes[0].End();

            ShaderManager.End();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}

