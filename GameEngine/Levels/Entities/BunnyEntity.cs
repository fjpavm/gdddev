// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BunnyEntity.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The bunny entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels.Entities
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The bunny entity.
    /// </summary>
    [LevelEntityBinding("Gdd.Game.Engine.Levels.Characters.Bunny, Gdd.Game.Engine")]
    [LevelEntityCategory("Characters")]
    public class BunnyEntity : AnimatedModelEntity
    {
        /// <summary>
        /// Patrol point to the left of the bunny
        /// </summary>
        public Vector2 PatrolPointLeft { get; set; }

        /// <summary>
        /// Patrol point to the right of the bunny
        /// </summary>
        public Vector2 PatrolPointRight { get; set; }

    }
}