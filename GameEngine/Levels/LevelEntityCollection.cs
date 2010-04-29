// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelEntityCollection.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The level entity collection.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using System.Collections.ObjectModel;

    /// <summary>
    /// The level entity collection.
    /// </summary>
    public class LevelEntityCollection : Collection<LevelEntity>
    {
        #region Properties

        /// <summary>
        /// Gets or sets Author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets Script.
        /// </summary>
        public string[] Script { get; set; }

        #endregion
    }
}