// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawableSceneComponentEntity.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The drawable scene component entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels.Entities
{
    /// <summary>
    /// The drawable scene component entity.
    /// </summary>
    [LevelEntityBinding("Gdd.Game.Engine.Scenes.DrawableSceneComponent, Gdd.Game.Engine")]
    public class DrawableSceneComponentEntity : SceneComponentEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether Visible.
        /// </summary>
        public bool Visible { get; set; }

        #endregion
    }
}