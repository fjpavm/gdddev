﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Ground.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The ground.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FarseerGames.FarseerPhysics;
    using FarseerGames.FarseerPhysics.Collisions;
    using FarseerGames.FarseerPhysics.Dynamics;
    using FarseerGames.FarseerPhysics.Factories;

    using Gdd.Game.Engine.Render;
    using Gdd.Game.Engine.Scenes;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The ground.
    /// </summary>
    public class Ground : StaticModel
    {
        #region Constants and Fields

        /// <summary>
        /// The ground bodies.
        /// </summary>
        private List<Body> GroundBodies;

        /// <summary>
        /// The ground geometries.
        /// </summary>
        private List<Geom> GroundGeometries;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Ground"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public Ground(Game game)
            : base(game)
        {
            // this.Rotation = Matrix.Identity;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Ground"/> class.
        /// </summary>
        protected Ground()
            : this(null)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The draw physics vertices.
        /// </summary>
        public override void DrawPhysicsVertices()
        {
            for (int i = 0; i < this.GroundBodies.Count; i++)
            {
                this.PhysicsBody = this.GroundBodies[i];
                this.PhysicsGeometry = this.GroundGeometries[i];
                base.DrawPhysicsVertices();
            }
        }

        /// <summary>
        /// The draw with effect.
        /// </summary>
        /// <param name="effect">
        /// The effect.
        /// </param>
        /// <param name="technique">
        /// The technique.
        /// </param>
        public override void DrawWithEffect(ShaderManager.EFFECT_ID effect, string technique)
        {
            this.Game.GraphicsDevice.RenderState.CullMode = CullMode.None;
            base.DrawWithEffect(effect, technique);
            this.Game.GraphicsDevice.RenderState.CullMode = CullMode.CullCounterClockwiseFace;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The load content.
        /// </summary>
        protected override void LoadContent()
        {
            // base.LoadContent();
            this.ObjectModel = this.Game.Content.Load<Model>(this.modelName);

            ShaderManager.AddEffect(ShaderManager.EFFECT_ID.STATICMODEL, "Effects\\StaticModel", this.Game);
            this.DefaultEffectID = ShaderManager.EFFECT_ID.STATICMODEL;
            this.DefaultTechnique = "StaticModelTechnique";

            foreach (Effect effect in this.ObjectModel.Meshes.SelectMany(mesh => mesh.Effects))
            {
                if (effect.Parameters["Texture"] != null)
                {
                    this.ModelTextures.Add(effect.Parameters["Texture"].GetValueTexture2D());
                }
                else if (effect.Parameters["BasicTexture"] != null)
                {
                    this.ModelTextures.Add(effect.Parameters["BasicTexture"].GetValueTexture2D());
                }
            }

            var modelBuffer = new List<Vector3>();

            int sizeofVertex = GetSizeOfMesh(this.ObjectModel);

            Vector2[] temp;
            this.GroundBodies = new List<Body>();
            this.GroundGeometries = new List<Geom>();
            Vertices vertices;

            foreach (ModelMesh mesh in this.ObjectModel.Meshes)
            {
                var meshBuffer = new MyStruct[mesh.VertexBuffer.SizeInBytes / sizeofVertex];
                var indexBuffer = new short[mesh.IndexBuffer.SizeInBytes / sizeof(short)];
                mesh.VertexBuffer.GetData(meshBuffer);

                // modelBuffer.AddRange(meshBuffer.Select(v => new Vector3(v.Position.Z, v.Position.Y, v.Position.X)));
                mesh.IndexBuffer.GetData(indexBuffer);

                for (int i = 0; i < indexBuffer.Length; i += 3)
                {
                    if (Math.Abs(meshBuffer[indexBuffer[i]].Position.X - meshBuffer[indexBuffer[i + 1]].Position.X) <
                        0.05f &&
                        Math.Abs(meshBuffer[indexBuffer[i + 1]].Position.X - meshBuffer[indexBuffer[i + 2]].Position.X) <
                        0.05f)
                    {
                        temp = new[]
                            {
                               new Vector2(meshBuffer[indexBuffer[i]].Position.Z, meshBuffer[indexBuffer[i]].Position.Y), 
                              new Vector2(
                                  meshBuffer[indexBuffer[i + 1]].Position.Z, meshBuffer[indexBuffer[i + 1]].Position.Y), 
                              new Vector2(
                                  meshBuffer[indexBuffer[i + 2]].Position.Z, meshBuffer[indexBuffer[i + 2]].Position.Y)
                            };

                        vertices = new Vertices(ref temp);
                        vertices.SubDivideEdges(2.0f);

                        this.PhysicsBody = BodyFactory.Instance.CreatePolygonBody(
                            SceneManager.physicsSimulator, vertices, 1);
                        this.PhysicsGeometry = GeomFactory.Instance.CreatePolygonGeom(
                            SceneManager.physicsSimulator, this.PhysicsBody, vertices, 0.0f);

                        this.PhysicsGeometry.FrictionCoefficient = 4.0f;

                        this.PhysicsGeometry.CollisionCategories = CollisionCategory.Cat1;
                        this.PhysicsGeometry.CollidesWith = CollisionCategory.All & ~CollisionCategory.Cat1;

                        this.offset = this.PhysicsBody.Position;
                        this.PhysicsBody.Position = this.Position2D + this.offset;

                        this.PhysicsBody.IsStatic = true;

                        this.GroundGeometries.Add(this.PhysicsGeometry);
                        this.GroundBodies.Add(this.PhysicsBody);
                    }
                }
            }
        }

        #endregion
    }
}