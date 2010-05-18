// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackgroundLayer.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The background layer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using System;

    using Gdd.Game.Engine.Scenes;

    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The background layer.
    /// </summary>
    public class BackgroundLayer
    {
        #region Constants and Fields

        /// <summary>
        /// The scene.
        /// </summary>
        private readonly Scene scene;

        /// <summary>
        /// The image.
        /// </summary>
        private Texture2D image;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundLayer"/> class.
        /// </summary>
        /// <param name="s">
        /// The s.
        /// </param>
        public BackgroundLayer(Scene s)
        {
            this.scene = s;
            this.HeightBias = 1;
            this.WidthBias = 1;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Filename.
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets HeightBias.
        /// </summary>
        public float HeightBias { get; set; }

        /// <summary>
        /// Gets Image.
        /// </summary>
        public Texture2D Image
        {
            get
            {
                return this.image;
            }
        }

        /// <summary>
        /// Gets or sets WidthBias.
        /// </summary>
        public float WidthBias { get; set; }

        public float ScrollSpeed { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The load content.
        /// </summary>
        public void LoadContent()
        {
            this.image = this.scene.Game.Content.Load<Texture2D>(this.Filename);
        }

        #endregion
    }
}