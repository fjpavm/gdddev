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
    using System.Collections.Generic;

    using Gdd.Game.Engine.Levels.Characters;
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
                    new BackgroundLayer(s) { Filename = "Levels/clouds", ScrollSpeed = 0.5f,WidthBias = 1/10.0f},
                    new BackgroundLayer(s) { Filename = "Levels/mountain", ScrollSpeed = 5.0f, WidthBias = 1 / 20.0f},
                    new BackgroundLayer(s) { Filename = "Levels/hills", ScrollSpeed = 30.0f, WidthBias = 1 / 30.0f}
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

            var p = new Plane(-Vector3.UnitZ, -10.0f);

            foreach (BackgroundLayer backgroundLayer in this.Backgrounds)
            {
                if (backgroundLayer.Image == null)
                {
                    continue;
                }

                Ray leftRay = this.scene.Camera.Unproject(0, this.scene.Game.GraphicsDevice.Viewport.Height / 2);
                Vector3? left3D = leftRay.IntersectsAt(p);
                float left = left3D.HasValue ? left3D.Value.X : this.min;

                Ray rightRay = this.scene.Camera.Unproject(
                    this.scene.Game.GraphicsDevice.Viewport.Width - 1, this.scene.Game.GraphicsDevice.Viewport.Height / 2);
                Vector3? right3D = rightRay.IntersectsAt(p);
                float right = right3D.HasValue ? right3D.Value.X : this.max;

                float offset = -((Hero.GetHeroPosition().X - this.min) * backgroundLayer.ScrollSpeed * 0.02f) %
                               backgroundLayer.Image.Width;

                float imgWidth = backgroundLayer.Image.Width * backgroundLayer.WidthBias;
                float screenLeft = this.min - backgroundLayer.Image.Width;
                int num = (int)((left - screenLeft) / imgWidth);
                left = screenLeft + (num * imgWidth);
                num = (int)(((right - left - offset) / imgWidth) + 2);

                int? prevRightX = null;
                for (int i = 0; i < num; i++)
                {
                    float imgLeft = left + offset + i * imgWidth;
                    float imgRight = imgLeft + imgWidth;
                    var scrLeft = this.scene.Camera.Project(new Vector3(imgLeft, 0, -10));
                    var scrRight = this.scene.Camera.Project(new Vector3(imgRight, 0, -10));
                    Rectangle destRect = new Rectangle(
                        prevRightX ?? (int)scrLeft.X,
                        0,
                        (int)(scrRight.X - scrLeft.X),
                        this.scene.Game.GraphicsDevice.Viewport.Height - 1);
                    spriteBatch.Draw(backgroundLayer.Image, destRect, Color.White);
                    prevRightX = destRect.X + destRect.Width - 1;
                }
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