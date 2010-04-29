using Gdd.Game.Engine.Menu;
using FarseerGames.FarseerPhysics;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SceneManager.cs" company="UAD">
//   game Design and Development
// </copyright>
// <summary>
//   The scene manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Scenes
{
    using System.Collections.Generic;
    using System.Linq;

    using Render;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The scene manager.
    /// </summary>
    public class SceneManager
    {
        #region Constants and Fields
        
        public enum SCENE_ID {MAIN_MENU, MAIN_GAME, IN_GAME_MENU};
        
        public static void ChangeScene(SCENE_ID id){

            if(scenes.Count-1 >= (int)id){
                SetCurrentScene(scenes[(int)id]);
            }

        }

        /// <summary>
        /// The scenes.
        /// </summary>
        private static SceneCollection scenes;

        /// <summary>
        /// The overlay vertex array.
        /// </summary>
        private static VertexPositionColor[] overlayVertexArray;

        /// <summary>
        /// The vb.
        /// </summary>
        private static VertexBuffer vb;

        /// <summary>
        /// The vd.
        /// </summary>
        private static VertexDeclaration vd;

        /// <summary>
        /// The game instance of the scenemanager
        /// </summary>
        private static Microsoft.Xna.Framework.Game game;

        public static PhysicsSimulator physicsSimulator;
        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SceneManager"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        /// <param name="z">
        /// The z.
        /// </param>
        public static void Construct(Microsoft.Xna.Framework.Game game)
        {
            Z_POSITION = -10.0f;
            scenes = new SceneCollection();
            
            physicsSimulator = new PhysicsSimulator(new Vector2(0.0f, -10.0f));
            
            SceneManager.game = game;
        }

        #endregion

        #region Properties

        /// <summary>
        /// used for drawing objects in the correct z- position
        /// </summary>
        public static float Z_POSITION { get; private set; }

        /// <summary>
        /// Gets Scenes.
        /// </summary>
        public static SceneCollection Scenes
        {
            get
            {
                return scenes;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The add scene.
        /// </summary>
        /// <param name="scene">
        /// The scene.
        /// </param>
        public static void AddScene(Scene scene)
        {
            scene.Visible = false;
            
            scenes.Add(scene);
            
            if (scene.TopMost && vd == null)
            {
                // set up the vertices and add the effect
                overlayVertexArray = new VertexPositionColor[4];

                overlayVertexArray[0] =
                    new VertexPositionColor(
                        new Vector3(
                            -game.GraphicsDevice.Viewport.Width / 2.0f, 
                            -game.GraphicsDevice.Viewport.Height / 2.0f, 
                            0.0f), 
                        Color.TransparentBlack);
                overlayVertexArray[1] =
                    new VertexPositionColor(
                        new Vector3(
                            -game.GraphicsDevice.Viewport.Width / 2.0f, 
                            game.GraphicsDevice.Viewport.Height / 2.0f, 
                            0.0f), 
                        Color.TransparentBlack);
                overlayVertexArray[2] =
                    new VertexPositionColor(
                        new Vector3(
                            game.GraphicsDevice.Viewport.Width / 2.0f, 
                            -game.GraphicsDevice.Viewport.Height / 2.0f, 
                            0.0f), 
                        Color.TransparentBlack);
                overlayVertexArray[3] =
                    new VertexPositionColor(
                        new Vector3(
                            game.GraphicsDevice.Viewport.Width / 2.0f, 
                            game.GraphicsDevice.Viewport.Height / 2.0f, 
                            0.0f), 
                        Color.TransparentBlack);

                vd = new VertexDeclaration(game.GraphicsDevice, VertexPositionColor.VertexElements);

                vb = new VertexBuffer(
                    game.GraphicsDevice, VertexPositionColor.SizeInBytes * 4, BufferUsage.None);
                vb.SetData(overlayVertexArray);

                Vector4 overlayColor = Color.TransparentBlack.ToVector4();
                overlayColor.W = 0.85f;

                ShaderManager.AddEffect(ShaderManager.EFFECT_ID.OVERLAY, "overlay", game);
                ShaderManager.SetCurrentEffect(ShaderManager.EFFECT_ID.OVERLAY);
                ShaderManager.SetValue("OverlayColor", overlayColor);
                ShaderManager.CommitChanges();
            }
        }

        /// <summary>
        /// The draw.
        /// </summary>
        /// <param name="gameTime">
        /// The gameTime.
        /// </param>
        public static void Draw(GameTime gameTime)
        {
            // draw this in the correct order so the scenes behind the topmost scene get drawn behind the topmost scene
            foreach (Scene scene in GetVisibleScenes())
            {
                if (scene.TopMost)
                {
                    game.GraphicsDevice.RenderState.AlphaBlendEnable = true;
                    game.GraphicsDevice.RenderState.SourceBlend = Blend.BothSourceAlpha;
                    game.GraphicsDevice.RenderState.DestinationBlend = Blend.DestinationColor;

                    ShaderManager.SetCurrentEffect(ShaderManager.EFFECT_ID.OVERLAY);
                    ShaderManager.GetCurrentEffectGraphicsDevice().VertexDeclaration = vd;
                    ShaderManager.SetCurrentTechnique("Overlay");
                    ShaderManager.Begin();
                    
                    foreach (EffectPass pass in ShaderManager.GetEffectPasses())
                    {
                        pass.Begin();
                            ShaderManager.GetCurrentEffectGraphicsDevice().Vertices[0].SetSource(vb, 0, VertexPositionColor.SizeInBytes);
                            ShaderManager.GetCurrentEffectGraphicsDevice().DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
                        pass.End();
                    }

                    ShaderManager.End();

                    game.GraphicsDevice.RenderState.AlphaBlendEnable = false;
                }

                scene.Draw(gameTime);
            }
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        public static void Initialize()
        {
            foreach (Scene scene in scenes)
            {
                scene.Initialize();
            }
        }

        /// <summary>
        /// The set current scene.
        /// </summary>
        /// <param name="scene">
        /// The scene.
        /// </param>
        public static void SetCurrentScene(Scene scene)
        {
            if (scenes.Contains(scene) && !scene.Visible)
            {
                // want to be able to have multiple transparent scenes overlaid on the nontransparent scene
                if (!scene.Transparent)
                {
                    foreach (Scene visibleScene in GetVisibleScenes())
                    {
                        visibleScene.Visible = false;
                    }
                }

                scene.Visible = true;

                if (scene.MainGameScene)
                {
                    var ingameMenu = new InGameMenu(scene.Game);
                    ingameMenu.Initialize();
                    AddScene(ingameMenu);
                }
                else if(!(scene is InGameMenu)){
                    scenes.Remove((from s in scenes where s is InGameMenu select s).FirstOrDefault<Scene>());
                }
            }
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The gameTime.
        /// </param>
        public static void Update(GameTime gameTime)
        {
            // go through the list in reverse so we stop at the top most scene
            foreach (Scene scene in GetVisibleScenes().Reverse())
            {
                scene.Update(gameTime);
                if (scene.TopMost)
                {
                    break;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The get visible scenes.
        /// </summary>
        /// <returns>
        /// Visible scenes.
        /// </returns>
        private static IEnumerable<Scene> GetVisibleScenes()
        {
            return from scene in scenes where scene.Visible select scene;
        }

        #endregion
    }
}