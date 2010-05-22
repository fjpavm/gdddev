// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CameraPositionChangedEventArgs.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The camera position changed event args.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.LevelEditor
{
    using System;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// The camera position changed event args.
    /// </summary>
    public class CameraPositionChangedEventArgs : EventArgs
    {
        #region Constants and Fields

        /// <summary>
        /// The camera position.
        /// </summary>
        private readonly Vector2 cameraPosition;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraPositionChangedEventArgs"/> class.
        /// </summary>
        /// <param name="position">
        /// The position.
        /// </param>
        public CameraPositionChangedEventArgs(Vector2 position)
        {
            this.cameraPosition = position;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets CameraPosition.
        /// </summary>
        public Vector2 CameraPosition
        {
            get
            {
                return this.cameraPosition;
            }
        }

        #endregion
    }
}