// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DrawableSceneComponent.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The drawable scene component.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Scenes
{
    using System;
    using System.Collections.Generic;

    using FarseerGames.FarseerPhysics.Collisions;
    using FarseerGames.FarseerPhysics.Dynamics;

    using Gdd.Game.Engine.Render;
    using Gdd.Game.Engine.Scenes.Lights;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The drawable scene component.
    /// </summary>
    public class DrawableSceneComponent : SceneComponent, IDrawable
    {
        #region Constants and Fields

        /// <summary>
        /// The adjacent drawable scene components.
        /// </summary>
        protected List<DrawableSceneComponent> adjacentDrawableSceneComponents;

        /// <summary>
        /// The adjacent scene components.
        /// </summary>
        protected List<SceneComponent> adjacentSceneComponents;

        /// <summary>
        /// The is loaded.
        /// </summary>
        protected bool isLoaded;

        /// <summary>
        /// The point light colors.
        /// </summary>
        private readonly Vector4[] pointLightColors;

        /// <summary>
        /// The pointlights
        /// </summary>
        private readonly Vector3[] pointLightPositions;

        /// <summary>
        /// The i d_ roller.
        /// </summary>
        private static int ID_ROLLER = 1;

        /// <summary>
        /// The device service.
        /// </summary>
        private IGraphicsDeviceService deviceService;

        /// <summary>
        /// The draw order.
        /// </summary>
        private int drawOrder;

        /// <summary>
        /// The initialized.
        /// </summary>
        private bool initialized;

        /// <summary>
        /// The visible.
        /// </summary>
        private bool visible;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DrawableSceneComponent"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public DrawableSceneComponent(Game game)
            : base(game)
        {
            this.adjacentDrawableSceneComponents = new List<DrawableSceneComponent>();
            this.adjacentSceneComponents = new List<SceneComponent>();

            this.pointLightPositions = new Vector3[4];
            this.pointLightColors = new Vector4[4];

            this.ID = ID_ROLLER++;
        }

        #endregion

        #region Events

        /// <summary>
        /// The draw order changed.
        /// </summary>
        public event EventHandler DrawOrderChanged;

        /// <summary>
        /// The visible changed.
        /// </summary>
        public event EventHandler VisibleChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets DefaultEffectID.
        /// </summary>
        public ShaderManager.EFFECT_ID DefaultEffectID { get; protected set; }

        /// <summary>
        /// Gets or sets DefaultTechnique.
        /// </summary>
        public string DefaultTechnique { get; protected set; }

        /// <summary>
        /// Gets or sets DrawOrder.
        /// </summary>
        public int DrawOrder
        {
            get
            {
                return this.drawOrder;
            }

            set
            {
                if (this.drawOrder != value)
                {
                    this.drawOrder = value;
                    this.OnDrawOrderChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets GraphicsDevice.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public GraphicsDevice GraphicsDevice
        {
            get
            {
                if (this.deviceService == null)
                {
                    throw new InvalidOperationException("Property Cannot Be Called Before Initialize");
                }

                return this.deviceService.GraphicsDevice;
            }
        }

        /// <summary>
        /// The DrawableObjects ID 
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Gets or sets PhysicsBody.
        /// </summary>
        public Body PhysicsBody { get; protected set; }

        /// <summary>
        /// Gets or sets PhysicsGeometry.
        /// </summary>
        public Geom PhysicsGeometry { get; protected set; }

        /// <summary>
        /// Gets or sets Texture.
        /// </summary>
        public Texture2D Texture { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether Visible.
        /// </summary>
        public bool Visible
        {
            get
            {
                return this.visible;
            }

            set
            {
                if (this.visible != value)
                {
                    this.visible = value;
                    this.OnVisibleChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets texture.
        /// </summary>
        public Texture2D texture { get; protected set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The add adjacent drawable scene component.
        /// </summary>
        /// <param name="dsc">
        /// The dsc.
        /// </param>
        public void AddAdjacentDrawableSceneComponent(DrawableSceneComponent dsc)
        {
            this.adjacentDrawableSceneComponents.Add(dsc);
        }

        /// <summary>
        /// The add adjacent scene component.
        /// </summary>
        /// <param name="sc">
        /// The sc.
        /// </param>
        public void AddAdjacentSceneComponent(SceneComponent sc)
        {
            this.adjacentSceneComponents.Add(sc);
        }

        /// <summary>
        /// The clear adjacency lists.
        /// </summary>
        public void ClearAdjacencyLists()
        {
            if (this.adjacentSceneComponents != null)
            {
                this.adjacentSceneComponents.Clear();
            }

            if (this.adjacentDrawableSceneComponents != null)
            {
                this.adjacentDrawableSceneComponents.Clear();
            }
        }

        /// <summary>
        /// The draw physics vertices.
        /// </summary>
        public virtual void DrawPhysicsVertices()
        {
            // draw the 2d representation of the model
            ShaderManager.AddEffect(ShaderManager.EFFECT_ID.SIMPLE, "SimpleEffect", this.Game);

            VertexPositionColor[] verticesPC;

            verticesPC = new VertexPositionColor[this.PhysicsGeometry.WorldVertices.Count + 1];
            for (int i = 0; i < this.PhysicsGeometry.WorldVertices.Count; i++)
            {
                verticesPC[i] =
                    new VertexPositionColor(
                        new Vector3(this.PhysicsGeometry.WorldVertices[i], SceneManager.Z_POSITION), Color.Green);
            }

            verticesPC[this.PhysicsGeometry.WorldVertices.Count] =
                new VertexPositionColor(
                    new Vector3(this.PhysicsGeometry.WorldVertices[0], SceneManager.Z_POSITION), Color.Green);

            var vd = new VertexDeclaration(this.Game.GraphicsDevice, VertexPositionColor.VertexElements);
            var VB = new VertexBuffer(
                this.Game.GraphicsDevice, 
                VertexPositionColor.SizeInBytes * (this.PhysicsGeometry.WorldVertices.Count + 1), 
                BufferUsage.None);
            VB.SetData(verticesPC);

            ShaderManager.SetCurrentEffect(ShaderManager.EFFECT_ID.SIMPLE);

            ShaderManager.SetValue("WVP", this.scene.Camera.View * this.scene.Camera.Perspective);

            ShaderManager.SetValue("Color", Color.Black.ToVector4());

            ShaderManager.SetCurrentTechnique("SimpleTechnique");
            ShaderManager.GetCurrentEffectGraphicsDevice().VertexDeclaration = vd;
            ShaderManager.Begin();
            foreach (EffectPass pass in ShaderManager.GetEffectPasses())
            {
                pass.Begin();
                ShaderManager.GetCurrentEffectGraphicsDevice().Vertices[0].SetSource(
                    VB, 0, VertexPositionColor.SizeInBytes);
                ShaderManager.GetCurrentEffectGraphicsDevice().DrawPrimitives(
                    PrimitiveType.LineStrip, 0, this.PhysicsGeometry.WorldVertices.Count);
                pass.End();
            }

            ShaderManager.End();
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
        public virtual void DrawWithEffect(ShaderManager.EFFECT_ID effect, string technique)
        {
        }

        /// <summary>
        /// The draw with technique.
        /// </summary>
        /// <param name="technique">
        /// The technique.
        /// </param>
        public virtual void DrawWithTechnique(string technique)
        {
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// </exception>
        public override void Initialize()
        {
            base.Initialize();
            if (!this.initialized)
            {
                this.deviceService =
                    this.Game.Services.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
                if (this.deviceService == null)
                {
                    throw new InvalidOperationException("Missing GraphicsDevice Service");
                }

                this.deviceService.DeviceCreated += this.DeviceCreated;
                this.deviceService.DeviceDisposing += this.DeviceDisposing;
                if (this.deviceService.GraphicsDevice != null)
                {
                    this.LoadContent();
                }
            }

            this.initialized = true;
        }

        #endregion

        #region Implemented Interfaces

        #region IDrawable

        /// <summary>
        /// Draws the object with the default effect.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public virtual void Draw(GameTime gameTime)
        {
#if !SHOW_ONLY_PHYSICS
            int nrOfPointLights = -1;

            if (this.adjacentSceneComponents != null)
            {
                foreach (PointLight p in this.adjacentSceneComponents)
                {
                    nrOfPointLights++;
                    this.pointLightColors[nrOfPointLights] = p.Color.ToVector4();
                    this.pointLightPositions[nrOfPointLights] = p.Position3D;
                    if (nrOfPointLights == 3)
                    {
                        break;
                    }
                }
            }

            ShaderManager.SetValue("InverseTransposeWorld", this.InverseTransposeWorld);
            ShaderManager.SetValue("PointLightColors", this.pointLightColors);
            ShaderManager.SetValue("PointLightPositionsW", this.pointLightPositions);
            ShaderManager.SetValue("PointLightCount", nrOfPointLights);

            this.DrawWithEffect(this.DefaultEffectID, this.DefaultTechnique);
#endif
#if SHOW_PHYSICS
            this.DrawPhysicsVertices();
#endif
        }

        #endregion

        #endregion

        #region Methods

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="disposing">
        /// The disposing.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.UnloadContent();
                if (this.deviceService != null)
                {
                    this.deviceService.DeviceCreated -= this.DeviceCreated;
                    this.deviceService.DeviceDisposing -= this.DeviceDisposing;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// The load content.
        /// </summary>
        protected virtual void LoadContent()
        {
            this.isLoaded = true;
        }

        /// <summary>
        /// The on draw order changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        protected virtual void OnDrawOrderChanged(object sender, EventArgs args)
        {
            if (this.DrawOrderChanged != null)
            {
                this.DrawOrderChanged(this, args);
            }
        }

        /// <summary>
        /// The on visible changed.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="args">
        /// The args.
        /// </param>
        protected virtual void OnVisibleChanged(object sender, EventArgs args)
        {
            if (this.VisibleChanged != null)
            {
                this.VisibleChanged(this, args);
            }
        }

        /// <summary>
        /// The unload content.
        /// </summary>
        protected virtual void UnloadContent()
        {
        }

        /// <summary>
        /// The device created.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DeviceCreated(object sender, EventArgs e)
        {
            this.LoadContent();
        }

        /// <summary>
        /// The device disposing.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void DeviceDisposing(object sender, EventArgs e)
        {
            this.UnloadContent();
        }

        #endregion
    }
}