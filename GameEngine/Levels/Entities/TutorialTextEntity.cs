// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TutorialTextEntity.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The tutorial text entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels.Entities
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The tutorial text entity.
    /// </summary>
    [LevelEntityBinding("Gdd.Game.Engine.Levels.Information.TutorialText, Gdd.Game.Engine")]
    [LevelEntityCategory("2D")]
    public class TutorialTextEntity : DrawableSceneComponentEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets BodyText.
        /// </summary>
        public string BodyText { get; set; }

        /// <summary>
        /// Gets or sets HeaderText.
        /// </summary>
        public string HeaderText { get; set; }

        /// <summary>
        /// Gets or sets TextBoxSize.
        /// </summary>
        public Vector2 TextBoxSize { get; set; }

        #endregion
    }
}