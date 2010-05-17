// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Button.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The texture type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.GUI
{
    using Gdd.Game.Engine.Levels;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The texture type.
    /// </summary>
    public enum TextureType
    {
        /// <summary>
        /// The normal.
        /// </summary>
        Normal, 

        /// <summary>
        /// The selected.
        /// </summary>
        Selected
    }

    /// <summary>
    /// The button type.
    /// </summary>
    public enum ButtonType
    {
        /// <summary>
        /// The model button.
        /// </summary>
        ModelButton, 

        /// <summary>
        /// The confirm button.
        /// </summary>
        ConfirmButton
    }

    /// <summary>
    /// The button.
    /// </summary>
    public class Button
    {
        #region Constants and Fields

        /// <summary>
        /// The button_texture_normal.
        /// </summary>
        private readonly Texture2D buttonTextureNormal;

        /// <summary>
        /// The button_texture_selected.
        /// </summary>
        private readonly Texture2D buttonTextureSelected;

        /// <summary>
        /// The game.
        /// </summary>
        private readonly Game game;

        /// <summary>
        /// The button_texture.
        /// </summary>
        private Texture2D buttonTexture;

        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch spriteBatch;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Button"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        /// <param name="normalImage">
        /// The normal_image.
        /// </param>
        /// <param name="selectedImage">
        /// The selected_image.
        /// </param>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <param name="w">
        /// The w.
        /// </param>
        /// <param name="h">
        /// The h.
        /// </param>
        /// <param name="guiModelName">
        /// The gui_model_name.
        /// </param>
        /// <param name="geoType">
        /// The geo type.
        /// </param>
        public Button(
            Game game, 
            string normalImage, 
            string selectedImage, 
            int x, 
            int y, 
            int w, 
            int h, 
            string guiModelName, 
            GeometryType geoType)
        {
            this.game = game;

            this.spriteBatch = (SpriteBatch)this.game.Services.GetService(typeof(SpriteBatch));
            this.buttonTexture = this.game.Content.Load<Texture2D>(@"Textures\" + normalImage);
            this.buttonTextureNormal = this.buttonTexture;
            this.buttonTextureSelected = this.game.Content.Load<Texture2D>(@"Textures\" + selectedImage);

            this.ButtonType = ButtonType.ModelButton;
            this.GeometryType = geoType;

            this.GuiModelName = guiModelName;

            this.X = x;
            this.Y = y;
            this.Width = w;
            this.Height = h;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets ButtonType.
        /// </summary>
        public ButtonType ButtonType { get; set; }

        /// <summary>
        /// Gets GeometryType.
        /// </summary>
        public GeometryType GeometryType { get; private set; }

        /// <summary>
        /// Gets or sets GuiModelName.
        /// </summary>
        public string GuiModelName { get; set; }

        /// <summary>
        /// Gets or sets Height.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets Width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets X.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets Y.
        /// </summary>
        public int Y { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The change image.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        public void ChangeImage(TextureType type)
        {
            switch (type)
            {
                case TextureType.Normal:
                    this.buttonTexture = this.buttonTextureNormal;
                    break;
                case TextureType.Selected:
                    this.buttonTexture = this.buttonTextureSelected;
                    break;
                default:
                    break;
            }
        }

        // public GUIModel GuiModel { get; set; }

        /// <summary>
        /// The draw.
        /// </summary>
        public void Draw()
        {
            if (this.spriteBatch == null)
            {
                this.spriteBatch = (SpriteBatch)this.game.Services.GetService(typeof(SpriteBatch));
            }

            this.spriteBatch.Draw(
                this.buttonTexture, new Rectangle(this.X, this.Y, this.Width, this.Height), Color.AntiqueWhite);
        }

        /// <summary>
        /// The is intersecting.
        /// </summary>
        /// <param name="x">
        /// The x.
        /// </param>
        /// <param name="y">
        /// The y.
        /// </param>
        /// <returns>
        /// The is intersecting.
        /// </returns>
        public bool IsIntersecting(int x, int y)
        {
            bool isIntersec = false;

            if (this.X < x && this.Y < y && x < this.X + this.Width && y < this.Y + this.Height)
            {
                isIntersec = true;
            }

            return isIntersec;
        }

        #endregion
    }
}