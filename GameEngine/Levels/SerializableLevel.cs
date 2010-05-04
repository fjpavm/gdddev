// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerializableLevel.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The serializable level.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The serializable level.
    /// </summary>
    public class SerializableLevel
    {
        #region Constants and Fields

        /// <summary>
        /// The level entity collection.
        /// </summary>
        private readonly LevelEntityCollection levelEntityCollection;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableLevel"/> class.
        /// </summary>
        public SerializableLevel()
        {
            this.levelEntityCollection = new LevelEntityCollection();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets LevelEntityCollection.
        /// </summary>
        public LevelEntityCollection LevelEntityCollection
        {
            get
            {
                return this.levelEntityCollection;
            }
        }

        /// <summary>
        /// Gets or sets Script.
        /// </summary>
        public string[] Script { get; set; }

        /// <summary>
        /// Gets or sets StartPosition.
        /// </summary>
        public Vector2 StartPosition { get; set; }

        #endregion
    }
}