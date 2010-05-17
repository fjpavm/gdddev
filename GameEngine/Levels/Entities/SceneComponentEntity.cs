// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SceneComponentEntity.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The scene component entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels.Entities
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The scene component entity.
    /// </summary>
    [LevelEntityBinding("Gdd.Game.Engine.Scenes.SceneComponent, Gdd.Game.Engine")]
    public class SceneComponentEntity : LevelEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether Enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Position2D.
        /// </summary>
        public Vector2 Position2D { get; set; }

        #endregion
    }
}