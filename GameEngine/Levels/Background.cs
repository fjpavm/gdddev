// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Background.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The background.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using Gdd.Game.Engine.Render;
    using Gdd.Game.Engine.Scenes;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The background.
    /// </summary>
    public class Background
    {
        #region Constants and Fields

        /// <summary>
        /// The scene.
        /// </summary>
        private readonly Scene scene;

        /// <summary>
        /// The max.
        /// </summary>
        private float max;

        /// <summary>
        /// The min.
        /// </summary>
        private float min;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Background"/> class.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        public Background(Scene s)
        {
            this.scene = s;
            ShaderManager.AddEffect(ShaderManager.EFFECT_ID.BACKGROUND, "Background", this.scene.Game);

            // TODO: Integrate into Level Editor
            this.Backgrounds = new[]
                {
                   new BackgroundLayer(s) { Filename = "Levels/sky", HeightBias = 1 }, 
                  new BackgroundLayer(s) { Filename = "Levels/cloudlayer4", HeightBias = 1 }, 
                  new BackgroundLayer(s) { Filename = "Levels/hillslayer3", HeightBias = 1 }, 
                  new BackgroundLayer(s) { Filename = "Levels/hillslayer2", HeightBias = 0.8f }, 
                  new BackgroundLayer(s) { Filename = "Levels/busheslayer1", HeightBias = 0.7f }, 
                };
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Backgrounds.
        /// </summary>
        public BackgroundLayer[] Backgrounds { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The draw.
        /// </summary>
        public void Draw()
        {
            if (this.Backgrounds == null || this.Backgrounds.Length == 0)
            {
                return;
            }

            var spriteBatch = (SpriteBatch)this.scene.Game.Services.GetService(typeof(SpriteBatch));
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.SaveState);

            // using a pixelshader with spritebatch - http://msdn.microsoft.com/en-us/library/bb313868.aspx
            ShaderManager.SetCurrentEffect(ShaderManager.EFFECT_ID.BACKGROUND);

            ShaderManager.Begin();
            ShaderManager.GetCurrentEffect().CurrentTechnique.Passes[0].Begin();

            int i = this.Backgrounds.Length - 1;

            foreach (BackgroundLayer backgroundLayer in this.Backgrounds)
            {
                if (backgroundLayer.Image == null)
                {
                    continue;
                }

                var p = new Plane(-Vector3.UnitZ, -10f * (i-- * 10 + 1));

                Ray leftRay = this.scene.Camera.Unproject(0, this.scene.Game.GraphicsDevice.Viewport.Height / 2);
                Vector3? left3D = leftRay.IntersectsAt(p);
                float left = left3D.HasValue ? left3D.Value.X : this.min;

                Ray rightRay = this.scene.Camera.Unproject(
                    this.scene.Game.GraphicsDevice.Viewport.Width, this.scene.Game.GraphicsDevice.Viewport.Height / 2);
                Vector3? right3D = rightRay.IntersectsAt(p);
                float right = right3D.HasValue ? right3D.Value.X : this.max;

                var height = (int)(this.scene.Game.GraphicsDevice.Viewport.Height * backgroundLayer.HeightBias);
                var destinationRectangle = new Rectangle(
                    0, 
                    this.scene.Game.GraphicsDevice.Viewport.Height - height, 
                    this.scene.Game.GraphicsDevice.Viewport.Width, 
                    height);

                float weight = (backgroundLayer.Image.Width / (this.max - this.min)) * backgroundLayer.WidthBias;
                var sourceRectangle = new Rectangle(
                    (int)((left - this.min) * weight), 0, (int)((right - left) * weight), backgroundLayer.Image.Height);

                // Use Immediate mode and our effect to draw the scene
                // again, using our pixel shader.
                spriteBatch.Draw(backgroundLayer.Image, destinationRectangle, sourceRectangle, Color.White);
            }

            spriteBatch.End();
            ShaderManager.GetCurrentEffect().CurrentTechnique.Passes[0].End();
            ShaderManager.End();
        }

        /// <summary>
        /// The load content.
        /// </summary>
        public void LoadContent()
        {
            if (this.Backgrounds == null || this.Backgrounds.Length == 0)
            {
                return;
            }

            foreach (BackgroundLayer backgroundLayer in this.Backgrounds)
            {
                backgroundLayer.LoadContent();
            }
        }

        /// <summary>
        /// The set bounds.
        /// </summary>
        /// <param name="min">
        /// The min.
        /// </param>
        /// <param name="max">
        /// The max.
        /// </param>
        public void SetBounds(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        #endregion
    }
}