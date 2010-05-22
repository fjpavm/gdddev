// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModelManager.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The model manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FarseerGames.FarseerPhysics.Collisions;

    using Gdd.Game.Engine.Physics;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The model manager.
    /// </summary>
    public static class ModelManager
    {
        #region Constants and Fields

        /// <summary>
        /// The model stuffs.
        /// </summary>
        private static readonly Dictionary<string, ModelStuff> modelStuffs;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="ModelManager"/> class.
        /// </summary>
        static ModelManager()
        {
            modelStuffs = new Dictionary<string, ModelStuff>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The load model.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <param name="game">
        /// The game.
        /// </param>
        /// <param name="matrix">
        /// The matrix.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// </exception>
        public static GddModel LoadModel(string content, Game game, Matrix matrix)
        {
            if (game == null)
            {
                throw new ArgumentNullException("game");
            }

            content = content.ToLowerInvariant();
            ModelStuff modelStuff;
            if (!modelStuffs.TryGetValue(content, out modelStuff))
            {
                modelStuff = new ModelStuff { Model = game.Content.Load<Model>(content) };

                if (modelStuff.Model.Tag != null)
                {
                    ModelToVertices.TransformAnimatedModel(modelStuff.Model, game, matrix);
                }

                modelStuffs.Add(content, modelStuff);

                foreach (Effect effect in modelStuff.Model.Meshes.SelectMany(mesh => mesh.Effects))
                {
                    if (effect.Parameters["Texture"] != null)
                    {
                        modelStuff.Textures.Add(effect.Parameters["Texture"].GetValueTexture2D());
                    }
                    else if (effect.Parameters["BasicTexture"] != null)
                    {
                        modelStuff.Textures.Add(effect.Parameters["BasicTexture"].GetValueTexture2D());
                    }
                }
            }

            var gddModel = new GddModel();
            if (modelStuff.Model.Tag == null)
            {
                VerticesMass vertices;
                if (!modelStuff.MatrixVertices.TryGetValue(matrix, out vertices))
                {
                    float mass;
                    var verticecMass = new VerticesMass
                        {
                            Vertices = ModelToVertices.TransformStaticModel(modelStuff.Model, game, matrix, out mass),
                            Mass = mass
                        };
                    modelStuff.MatrixVertices.Add(matrix, verticecMass);
                }

                gddModel.Vertices = modelStuff.MatrixVertices[matrix].Vertices;
                gddModel.Mass = modelStuff.MatrixVertices[matrix].Mass;
            }

            gddModel.Textures = modelStuff.Textures;
            gddModel.Model = modelStuff.Model;
            return gddModel;
        }

        #endregion

        /// <summary>
        /// The model stuff.
        /// </summary>
        private class ModelStuff
        {
            #region Constructors and Destructors

            /// <summary>
            /// Initializes a new instance of the <see cref="ModelStuff"/> class.
            /// </summary>
            public ModelStuff()
            {
                this.MatrixVertices = new Dictionary<Matrix, VerticesMass>();
                this.Textures = new List<Texture2D>();
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets MatrixVertices.
            /// </summary>
            public Dictionary<Matrix, VerticesMass> MatrixVertices { get; private set; }

            /// <summary>
            /// Gets or sets Model.
            /// </summary>
            public Model Model { get; set; }

            /// <summary>
            /// Gets Textures.
            /// </summary>
            public List<Texture2D> Textures { get; private set; }

            #endregion
        }

        /// <summary>
        /// The vertices mass.
        /// </summary>
        private class VerticesMass
        {
            #region Properties

            /// <summary>
            /// Gets or sets Mass.
            /// </summary>
            public float Mass { get; set; }

            /// <summary>
            /// Gets or sets Vertices.
            /// </summary>
            public Vertices Vertices { get; set; }

            #endregion
        }
    }
}