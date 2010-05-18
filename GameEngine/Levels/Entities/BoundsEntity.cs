// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoundsEntity.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The bounds entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels.Entities
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The bounds entity.
    /// </summary>
    [LevelEntityBinding("Gdd.Game.Engine.Levels.Bounds, Gdd.Game.Engine")]
    [LevelEntityCategory("Scripting")]
    public class BoundsEntity : DrawableSceneComponentEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets Size.
        /// </summary>
        public Vector2 Size { get; set; }

        #endregion
    }
}