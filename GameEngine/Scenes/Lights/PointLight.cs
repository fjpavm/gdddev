using FarseerGames.FarseerPhysics.Collisions;
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
    using Microsoft.Xna.Framework;

    /// <summary>
    /// This is a game component that extends Light
    /// </summary>
    public class PointLight : Light
    {
        /// <summary>
        /// Gets or sets Radius.
        /// </summary>
        public float Radius { get; set; }

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

        /// <summary>
        /// The initialize.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            Vector2 min = new Vector2(-this.Radius, -this.Radius); 
            Vector2 max = new Vector2(this.Radius, this.Radius); 
            this.aabb = new AABB(ref min, ref max);
        }
    }
}
