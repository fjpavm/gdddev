// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimatedModelEntity.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The animated model entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Gdd.Game.Engine.Animation;
namespace Gdd.Game.Engine.Levels.Entities
{
    /// <summary>
    /// The animated model entity.
    /// </summary>
    [LevelEntityBinding("Gdd.Game.Engine.Levels.AnimatedModel, Gdd.Game.Engine")]
    [LevelEntityCategory("Characters")]
    public class AnimatedModelEntity : StaticModelEntity
    {
    }
}