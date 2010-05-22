// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawTextEntity.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The draw text entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels.Entities
{
    /// <summary>
    /// The draw text entity.
    /// </summary>
    [LevelEntityBinding("Gdd.Game.Engine.DrawText, Gdd.Game.Engine")]
    [LevelEntityCategory("2D")]
    public class DrawTextEntity : DrawableSceneComponentEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets Text.
        /// </summary>
        public string Text { get; set; }

        #endregion
    }
}