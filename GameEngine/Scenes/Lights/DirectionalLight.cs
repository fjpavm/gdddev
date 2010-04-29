﻿﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectionalLight.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The directional light.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Scenes.Lights
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The directional light.
    /// </summary>
    public class DirectionalLight : Light
    {
        #region Constructors and Destructors

        public Vector3 Direction { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectionalLight"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public DirectionalLight(Game game)
            : base(game)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectionalLight"/> class.
        /// </summary>
        protected DirectionalLight()
            : base(null)
        {
        }

        #endregion
    }
}
