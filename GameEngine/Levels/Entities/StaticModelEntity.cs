// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaticModelEntity.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The static model entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels.Entities
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The static model entity.
    /// </summary>
    [LevelEntityBinding("Gdd.Game.Engine.Levels.StaticModel, Gdd.Game.Engine")]
    [LevelEntityCategory("Shapes")]
    public class StaticModelEntity : DrawableSceneComponentEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets GeometryType.
        /// </summary>
        public GeometryType GeometryType { get; set; }

        /// <summary>
        /// Gets or sets ModelDirection.
        /// </summary>
        public ModelDirection ModelDirection { get; set; }

        /// <summary>
        /// Gets or sets ModelName.
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Gets or sets PitchRotation.
        /// </summary>
        public float PitchRotation { get; set; }

        /// <summary>
        /// Gets or sets RollRotation.
        /// </summary>
        public float RollRotation { get; set; }

        /// <summary>
        /// Gets or sets ModelName.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Gets or sets YawRotation.
        /// </summary>
        public float YawRotation { get; set; }

        #endregion
    }
}