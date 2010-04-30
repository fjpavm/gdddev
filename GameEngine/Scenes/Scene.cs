using Gdd.Game.Engine.Render.Shadow;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Scene.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The s.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Scenes
{
    using System.Collections.Generic;
    using System.Linq;
    using Gdd.Game.Engine.Physics;

    using Common;

    using Input;

    using Levels.Characters;

    using Scenes.Lights;
    using Levels;

    using Render;
    using Render.Shadow;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    using Microsoft.Xna.Framework.Graphics;
    using Gdd.Game.Engine.GUI;

    /// <summary>
    /// The s.
    /// </summary>
    public class Scene : DrawableGameComponent
    {
        #region Constants and Fields

        /// <summary>
        /// The id.
        /// </summary>
        protected int ID;

        /// <summary>
        /// The directional light used in the s.
        /// </summary>
        protected DirectionalLight directionalLight;

        /// <summary>
        /// The i d_ roller.
        /// </summary>
        private static int ID_ROLLER = 0;

        /// <summary>
        /// The pip view port.
        /// </summary>
        private Viewport PIPViewPort;

        /// <summary>
        /// The b.
        /// </summary>
        private BoundingSphere b;

        /// <summary>
        /// The bounds.
        /// </summary>
        private BoundingSphere bounds;

        /// <summary>
        /// The camera.
        /// </summary>
        private Camera camera;

        /// <summary>
        /// The DepthBugger. Used for shadowmapping
        /// </summary>
        private DepthStencilBuffer depthBuffer;

        /// <summary>
        /// Is this Scene the main game s (for shadowmapping)
        /// </summary>
        private bool mainGameScene;

        /// <summary>
        /// The Rendertarget. Used for shadowmapping
        /// </summary>
        private RenderTarget2D renderTarget;

        /// <summary>
        /// The shadow map.
        /// </summary>
        private Texture2D shadowMap;

        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch spriteBatch;

        private GameGUI gameGUI = null;
        private bool gameGUI_Run = false;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Scene"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public Scene(Game game)
            : base(game)
        {
            this.Transparent = false;
            this.TopMost = false;
            this.ID = ID_ROLLER++;
            ObjectManager.SetUpLists(this.ID);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Camera.
        /// </summary>
        public Camera Camera
        {
            get
            {
                return this.camera;
            }

            set
            {
                this.camera = value;
            }
        }

        public List<SceneComponent> SceneComponents
        {
            get
            {
                return ObjectManager.sceneComponents[this.ID];
            }
        }

        public List<DrawableSceneComponent> DrawableSceneComponents
        {
            get
            {
                return ObjectManager.drawableSceneComponents[this.ID];
            }
        }

        /// <summary>
        /// Gets or sets InputManager.
        /// </summary>
        public InputManager InputManager { get; set; }

        /// <summary>
        /// Gets or sets Light.
        /// </summary>
        public DirectionalLight Light
        {
            get
            {
                return this.directionalLight;
            }

            set
            {
                this.directionalLight = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether MainGameScene.
        /// </summary>
        public bool MainGameScene
        {
            get
            {
                return this.mainGameScene;
            }

            set
            {
                this.mainGameScene = value;
                if (value)
                {
                    // Create a render target for our depth texture.  The Xbox supports
                    // SurfaceFormat.Single but PC video cards may not.
                    /*this.renderTarget = GfxComponent.CreateRenderTarget(
                        this.Game.GraphicsDevice, 1, SurfaceFormat.Single);
                    */
                    this.renderTarget = GfxComponent.CreateCustomRenderTarget(this.Game.GraphicsDevice, 1, SurfaceFormat.Single, MultiSampleType.None, 1500, 1500);
                    // Create a depth stencil buffer to match our render target.
                    // The Xbox supports Depth24Stencil8Single but 
                    // PC video cards may not.
                    /*this.depthBuffer = GfxComponent.CreateDepthStencil(
                        this.renderTarget, DepthFormat.Depth24Stencil8Single);
                    */
                    this.depthBuffer = GfxComponent.CreateDepthStencil(this.renderTarget, DepthFormat.Depth24Stencil8Single);

                    ShaderManager.AddEffect(ShaderManager.EFFECT_ID.SHADOWMAP, "ShadowMap", this.Game);
                    this.spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);

                    this.PIPViewPort = this.Game.GraphicsDevice.Viewport;
                    this.PIPViewPort.Height /= 4;
                    this.PIPViewPort.Width /= 4;
                    this.PIPViewPort.X = this.Game.GraphicsDevice.Viewport.Width - this.PIPViewPort.Width - 50;
                    this.PIPViewPort.Y = this.Game.GraphicsDevice.Viewport.Height - this.PIPViewPort.Height - 50;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether TopMost.
        /// </summary>
        public bool TopMost { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Transparent.
        /// </summary>
        public bool Transparent { get; set; }

        public GameGUI GameGUI { get { return this.gameGUI; } set { this.gameGUI = value; } }

        public bool GameGUIRun
        {
            get { return gameGUI_Run; }
            set
            {
                this.gameGUI_Run = value;
                if (this.gameGUI_Run)
                {
                    gameGUI.Run();
                }
                else
                {
                    gameGUI.Stop();
                }
            }
        }

        private Vector4 LightAmbient = new Vector4(0.5f, 0.5f, 0.5f, 1.0f);
        private Vector4 LightDiffuse = Color.White.ToVector4();
        #endregion

        #region Public Methods

        /// <summary>
        /// The add drawable game component.
        /// </summary>
        /// <param name="sceneComponent">
        /// The sceneComponent.
        /// </param>
        public virtual void AddComponent(SceneComponent sceneComponent)
        {
            sceneComponent.SetScene(this);
            if (sceneComponent is DrawableSceneComponent)
            {
                var dsc = (DrawableSceneComponent)sceneComponent;
                ObjectManager.AddDrawableSceneComponent(this.ID, ref dsc);
            }
            else
            {
                ObjectManager.AddSceneComponent(this.ID, ref sceneComponent);
            }
        }

        public bool IsGameComponentAddAlready(SceneComponent sceneComponent)
        {
            if (sceneComponent is DrawableSceneComponent)
            {
                var dsc = (DrawableSceneComponent)sceneComponent;
                return ObjectManager.drawableSceneComponents[this.ID].Contains(dsc);
            }
            else
            {
                return ObjectManager.sceneComponents[this.ID].Contains(sceneComponent);
            }
        }

        public void RemoveComponent(SceneComponent sceneComponent)
        {
            if (sceneComponent is DrawableSceneComponent)
            {
                DrawableSceneComponent dsc = (DrawableSceneComponent)sceneComponent;
                ObjectManager.RemoveDrawableSceneComponent(this.ID, ref dsc);
                //this.drawableSceneComponents.Add((DrawableSceneComponent)sceneComponent);
            }
            else
            {
                ObjectManager.RemoveSceneComponent(this.ID, ref sceneComponent);
                //this.sceneComponents.Add(sceneComponent);
            }
            this.Game.Components.Remove(sceneComponent);
        }

        /// <summary>
        /// Clears Scene Components
        /// </summary>
        public void ClearComponents()
        {
            IEnumerable<SceneComponent> sceneComponents =
                ObjectManager.drawableSceneComponents[this.ID].Select(c => (SceneComponent)c).Union(
                    ObjectManager.sceneComponents[this.ID]);
            /*  foreach (SceneComponent sceneComponent in sceneComponents)
              {
                  this.Game.Components.Remove(sceneComponent);
              }*/

            ObjectManager.ClearSceneComponents(this.ID);
        }

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Draw(GameTime gameTime)
        {
            // Calculate the shadowMap
            if (this.MainGameScene)
            {
                this.shadowMap = ShadowMapManager.CalculateShadowMapForScene(this.Game.GraphicsDevice, renderTarget, depthBuffer, -this.directionalLight.Direction, bounds, this.ID);
                this.Game.GraphicsDevice.Clear(Color.CornflowerBlue);
            }

            this.DrawBackground();

            if (this.MainGameScene)
            {
                DrawSceneComponent(gameTime);
              //  DrawSecondViewport(this.shadowMap);
            }

            if (gameGUI_Run)
            {
                gameGUI.Draw();
            }
        }

        protected virtual void DrawBackground()
        {

        }


        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Update(GameTime gameTime)
        {
            AdjacencyManager.PopulateAdjacency(this.ID);

            if (this.mainGameScene)
            {
                Audio.RepeatPlayBackgroundMusic();
            }
            if (this.InputManager != null)
            {
                this.InputManager.Update();
            }

            foreach (DrawableSceneComponent dsc in ObjectManager.drawableSceneComponents[this.ID])
            {
                dsc.Update(gameTime);
            }

            foreach (SceneComponent sc in ObjectManager.sceneComponents[this.ID])
            {
                sc.Update(gameTime);
            }

            if (this.camera != null)
            {
                bounds.Center = camera.LookAt;
                this.camera.Update(gameTime);
            }

            if (gameGUI_Run)
            {
                this.gameGUI.Update();
            }
            
            if (this.mainGameScene)
            {
                SceneManager.physicsSimulator.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The draw second viewport.
        /// </summary>
        /// <param name="sprite">
        /// The sprite.
        /// </param>
        private void DrawSecondViewport(Texture2D sprite)
        {
            if (this.spriteBatch == null)
            {
                return;
            }

            var rect = new Rectangle(
                0, 0, 300, 300);

            this.spriteBatch.Begin(SpriteBlendMode.None);
            this.spriteBatch.Draw(sprite, rect, Color.White);
            this.spriteBatch.End();
        }

        /// <summary>
        /// Draw the scene components
        /// </summary>
        private void DrawSceneComponent(GameTime gameTime)
        {
            foreach (DrawableSceneComponent dsc in ObjectManager.drawableSceneComponents[this.ID])
            {
                ShaderManager.SetCurrentEffect(dsc.DefaultEffectID);
                ShaderManager.SetValue("Texture", dsc.texture);
                ShaderManager.SetValue("life", Hero.GetHeroLife());
                ShaderManager.SetValue("ID", dsc.ID);
                ShaderManager.SetValue("LightDir", -this.directionalLight.Direction);                
                ShaderManager.SetValue("LightProj", ShadowMapManager.CalcLightProjection(bounds, -this.directionalLight.Direction, this.Game.GraphicsDevice.Viewport));
                ShaderManager.SetValue("LightView", Matrix.CreateLookAt(-this.directionalLight.Direction * bounds.Radius + bounds.Center, bounds.Center, Vector3.Up)); ;
                ShaderManager.SetValue("LightDiffuse", LightDiffuse);
                ShaderManager.SetValue("LightAmbient", LightAmbient);
                ShaderManager.SetValue("ShadowMapTexture", shadowMap);

                ShaderManager.SetValue("View", this.Camera.View);
                ShaderManager.SetValue("Projection", this.Camera.Perspective);

                dsc.Draw(gameTime);
            }
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        public override void Initialize()
        {
            // Calculate bounds of scene
            bounds = new BoundingSphere();
            foreach (DrawableSceneComponent dsc in ObjectManager.drawableSceneComponents[this.ID])
            {
                dsc.Initialize();
            }

            bounds.Radius = 120.0f;

            foreach (SceneComponent sc in ObjectManager.sceneComponents[this.ID])
            {
                sc.Initialize();
            }
            this.LoadContent();
        }
        #endregion
    }
}
