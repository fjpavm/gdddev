// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectedComponentChangedEventArgs.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The selected block changed event args.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.LevelEditor
{
    using System;

    using Gdd.Game.Engine.Scenes;

    /// <summary>
    /// The selected block changed event args.
    /// </summary>
    public class SelectedComponentChangedEventArgs : EventArgs
    {
        #region Constants and Fields

        /// <summary>
        /// The selected block.
        /// </summary>
        private readonly SceneComponent selectedComponent;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectedComponentChangedEventArgs"/> class.
        /// </summary>
        /// <param name="component">
        /// The selected block.
        /// </param>
        public SelectedComponentChangedEventArgs(SceneComponent component)
        {
            this.selectedComponent = component;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets SelectedComponent.
        /// </summary>
        public SceneComponent SelectedComponent
        {
            get
            {
                return this.selectedComponent;
            }
        }

        #endregion
    }
}