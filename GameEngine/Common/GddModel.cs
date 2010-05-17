// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GddModel.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The gdd model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Common
{
    using System;
    using System.Collections.Generic;

    using FarseerGames.FarseerPhysics.Collisions;

    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The gdd model.
    /// </summary>
    public class GddModel
    {
        #region Properties

        /// <summary>
        /// Gets or sets Mass.
        /// </summary>
        public float Mass { get; set; }

        /// <summary>
        /// Gets or sets Model.
        /// </summary>
        public Model Model { get; set; }

        /// <summary>
        /// Gets or sets Vertices.
        /// </summary>
        public Vertices Vertices { get; set; }

        public List<Texture2D> Textures
        {
            get; set;
        }

        #endregion
    }
}