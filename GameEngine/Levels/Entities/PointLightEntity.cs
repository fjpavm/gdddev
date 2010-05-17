// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawableSceneComponentEntity.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The point lightcomponent entity.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Gdd.Game.Engine.Levels.Entities
{
    /// <summary>
    /// The Pointlight component entity.
    /// </summary>
    [LevelEntityBinding("Gdd.Game.Engine.Scenes.Lights.PointLight, Gdd.Game.Engine")]
    [LevelEntityCategory("Lights")]
    public class PointLightEntity : DrawableSceneComponentEntity
    {

        #region Properties

        public float Radius { get; set; }

        public Color Color { get; set; }

        #endregion
    }
}