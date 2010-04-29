// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelComponentCollection.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The level block collection.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Gdd.Game.Engine.Scenes;

    /// <summary>
    /// The level block collection.
    /// </summary>
    public sealed class LevelComponentCollection : Collection<SceneComponent>
    {
        #region Constants and Fields

        /// <summary>
        /// The auto initialize.
        /// </summary>
        private bool autoInitialize;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelComponentCollection"/> class.
        /// </summary>
        /// <param name="autoInitialize">
        /// The auto initialize.
        /// </param>
        public LevelComponentCollection(bool autoInitialize)
        {
            this.autoInitialize = autoInitialize;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether AutoInitialize.
        /// </summary>
        public bool AutoInitialize
        {
            get
            {
                return this.autoInitialize;
            }

            set
            {
                this.autoInitialize = value;
            }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index of the element to get or set.
        /// </param>
        public SceneComponent this[string index]
        {
            get
            {
                return this.Where(levelBlock => levelBlock.Name == index).FirstOrDefault();
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get types of blocks that are in the collection
        /// </summary>
        /// <returns>
        /// Returns types of blocks that are in the collection.
        /// </returns>
        public IEnumerable<Type> GetBlockTypes()
        {
            return from lb in this group lb by lb.GetType() into t select t.Key;
        }

        /// <summary>
        /// The get drawable components.
        /// </summary>
        /// <returns>
        /// </returns>
        public IEnumerable<DrawableSceneComponent> GetDrawableComponents()
        {
            return from sceneComponent in this.Items
                   where sceneComponent is DrawableSceneComponent
                   select (DrawableSceneComponent)sceneComponent;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The insert item.
        /// </summary>
        /// <param name="index">
        /// The index.
        /// </param>
        /// <param name="item">
        /// The item.
        /// </param>
        protected override void InsertItem(int index, SceneComponent item)
        {
            base.InsertItem(index, item);
            if (this.autoInitialize)
            {
                item.Initialize();
            }
        }

        #endregion
    }
}