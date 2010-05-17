// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelEntityTypeBinding.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The level entity scene component type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using System;

    using Gdd.Game.Engine.Scenes;

    /// <summary>
    /// The level entity scene component type.
    /// </summary>
    public class LevelEntityTypeBinding
    {
        #region Constants and Fields

        /// <summary>
        /// The level entity type.
        /// </summary>
        private readonly Type levelEntityType;

        /// <summary>
        /// The scene component type.
        /// </summary>
        private readonly Type sceneComponentType;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelEntityTypeBinding"/> class.
        /// </summary>
        /// <param name="levelEntityType">
        /// The level entity type.
        /// </param>
        /// <param name="sceneComponentType">
        /// The scene component type.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        public LevelEntityTypeBinding(Type levelEntityType, Type sceneComponentType)
        {
            if (levelEntityType == null)
            {
                throw new ArgumentNullException("levelEntityType");
            }

            if (!levelEntityType.IsSubclassOf(typeof(LevelEntity)))
            {
                throw new ArgumentException(
                    "The class represented by levelEntityType must derive from LevelEntity.", "levelEntityType");
            }

            if (sceneComponentType == null)
            {
                throw new ArgumentNullException("sceneComponentType");
            }

            if (sceneComponentType != typeof(SceneComponent) && !sceneComponentType.IsSubclassOf(typeof(SceneComponent)))
            {
                throw new ArgumentException(
                    "The class represented by sceneComponentType must derive from SceneComponent.", "sceneComponentType");
            }

            this.levelEntityType = levelEntityType;
            this.sceneComponentType = sceneComponentType;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets LevelEntityType.
        /// </summary>
        public Type LevelEntityType
        {
            get
            {
                return this.levelEntityType;
            }
        }

        /// <summary>
        /// Gets SceneComponentType.
        /// </summary>
        public Type SceneComponentType
        {
            get
            {
                return this.sceneComponentType;
            }
        }

        #endregion
    }
}