// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PointLight.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   This is a game component that extends Light
// </summary>
// --------------------------------------------------------------------------------------------------------------------

 // --------------------------------------------------------------------------------------------------------------------
// <copyright file="PointLight.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   This is a game component that extends Light
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Scenes.Lights
{
    using FarseerGames.FarseerPhysics.Collisions;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// This is a game component that extends Light
    /// </summary>
    public class PointLight : Light
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PointLight"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public PointLight(Game game)
            : base(game)
        {
            this.pos3D.Z = -1.0f;
            this.Color = this.Color;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Radius.
        /// </summary>
        public float Radius { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The initialize.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            var min = new Vector2(-this.Radius, -this.Radius);
            var max = new Vector2(this.Radius, this.Radius);
            this.aabb = new AABB(ref min, ref max);
        }

        #endregion
    }
}