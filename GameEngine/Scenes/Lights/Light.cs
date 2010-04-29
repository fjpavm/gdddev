// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Light.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   This is a game component that implements SceneComponent
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Scenes.Lights
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// This is a game component that implements SceneComponent
    /// </summary>
    public abstract class Light : SceneComponent
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Light"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public Light(Game game)
            : base(game)
        {
            this.Color = Color.TransparentBlack;
            this.world = Matrix.Identity;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Color.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets Position3D.
        /// </summary>
        public override Vector3 Position3D
        {
            get
            {
                return base.Position3D;
            }

            set
            {
                this.pos3D = value;
            }
        }

        /// <summary>
        /// Gets or sets world.
        /// </summary>
        public Matrix world { get; set; }

        #endregion
    }
}