// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Level.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The level.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using System;
    using System.IO;
    using System.Linq;

    using Gdd.Game.Engine.Scenes;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// The level.
    /// </summary>
    public class Level : ICloneable
    {
        #region Constants and Fields

        /// <summary>
        /// The components.
        /// </summary>
        private readonly LevelComponentCollection components;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Level"/> class.
        /// </summary>
        /// <param name="autoInitialize">
        /// The auto initialize.
        /// </param>
        public Level(bool autoInitialize)
        {
            this.components = new LevelComponentCollection(autoInitialize);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Level"/> class.
        /// </summary>
        protected Level()
            : this(false)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets components.
        /// </summary>
        public LevelComponentCollection Components
        {
            get
            {
                return this.components;
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

        #region Public Methods

        /// <summary>
        /// The get block at.
        /// </summary>
        /// <param name="vector3">
        /// The vector 3.
        /// </param>
        /// <returns>
        /// First LevelBlock at the given coordinates or null.
        /// </returns>
        public SceneComponent GetBlockAt(Vector3 vector3)
        {
            return (from block in this.Components
                    let boundingBox = block.aabb
                    where
                        boundingBox.Min.X <= vector3.X && boundingBox.Min.Y <= vector3.Y &&
                        boundingBox.Max.X >= vector3.X && boundingBox.Max.Y >= vector3.Y
                    select block).FirstOrDefault();
        }

        #endregion

        #region Implemented Interfaces

        #region ICloneable

        /// <summary>
        /// ICloneable.Clone() implementation.
        /// </summary>
        /// <returns>
        /// Returns a deep-clone.
        /// </returns>
        public object Clone()
        {
            using (var memoryStream = new MemoryStream())
            {
                var levelSerializer = new LevelSerializer();
                levelSerializer.Serialize(memoryStream, this);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return levelSerializer.Deserialize(memoryStream, null);
            }
        }

        #endregion

        #endregion
    }
}