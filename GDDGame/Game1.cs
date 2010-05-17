// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Game1.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   This is the main type for your game
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game
{
    using System.Collections.Generic;

    using Gdd.Game.Engine;
    using Gdd.Game.Engine.AI;
    using Gdd.Game.Engine.Common;
    using Gdd.Game.Engine.GUI;
    using Gdd.Game.Engine.Levels;
    using Gdd.Game.Engine.Levels.Characters;
    using Gdd.Game.Engine.Levels.Information;
    using Gdd.Game.Engine.Menu;
    using Gdd.Game.Engine.Render.Shadow;
    using Gdd.Game.Engine.Scenes;
    using Gdd.Game.Engine.Scenes.Lights;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        #region Constants and Fields

        /// <summary>
        /// The graphics.
        /// </summary>
        private readonly GraphicsDeviceManager graphics;

        /// <summary>
        /// The camera.
        /// </summary>
        private MyCamera camera;

        /// <summary>
        /// The hero.
        /// </summary>
        private Hero hero;

        /// <summary>
        /// The camera.
        /// </summary>
        private CharacterFollowCamera heroCamera;

        /// <summary>
        /// Last keyboard state
        /// </summary>
        private KeyboardState lastState;

        /// <summary>
        /// The main menu.
        /// </summary>
        private MainMenu mainMenu;

        // FrankM: Just for testing;
        /// <summary>
        /// The monster.
        /// </summary>
        private AIMonster monster;

        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch spriteBatch;

        #endregion

        // private Character character1;
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Game1"/> class.
        /// </summary>
        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            //this.aiManager = new AIManager(this);
            this.Content.RootDirectory = "Content";

            // add the GFXComponent into the components
            this.Components.Add(new GfxComponent(this, this.graphics));

            this.graphics.MinimumPixelShaderProfile = ShaderProfile.PS_2_0;
            this.graphics.MinimumVertexShaderProfile = ShaderProfile.VS_2_0;

            SceneManager.Construct(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">
        /// Provides a snapshot of timing values.
        /// </param>
        protected override void Draw(GameTime gameTime)
        {
            this.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here     
            SceneManager.Draw(gameTime);

            // draws all objects in the components array
            base.Draw(gameTime);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // calls initialize for all objects in the components array
            base.Initialize();

            //this.SetupAI();
            this.SetupScenes();

            SceneManager.ChangeScene(SceneManager.SCENE_ID.MAIN_MENU);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);
            this.Services.AddService(typeof(SpriteBatch), this.spriteBatch);

            this.graphics.GraphicsDevice.PresentationParameters.IsFullScreen = true;

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">
        /// Provides a snapshot of timing values.
        /// </param>
        protected override void Update(GameTime gameTime)
        {
            // FrankM: just for testing 
            Message m;

            // Console.WriteLine("well well well well well well well well well" + character1.Position2D);
            // character1 = new Character(this, "mesh//hero03");
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                SceneManager.ChangeScene(SceneManager.SCENE_ID.IN_GAME_MENU);
                Audio.PauseBackgroundMusic();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                m = new Message
                    {
                        MessageType = MessageTypes.die, 
                        timeDelivery = gameTime.TotalGameTime.TotalSeconds, 
                        to = this.monster
                    };
                AIManager.messageQueue.sendMessage(m);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                m = new Message
                    {
                        MessageType = MessageTypes.resurect, 
                        timeDelivery = gameTime.TotalGameTime.TotalSeconds, 
                        to = this.monster
                    };
                AIManager.messageQueue.sendMessage(m);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                this.hero.Position2D = new Vector2(this.hero.Position2D.X - 0.1f, this.hero.Position2D.Y);
                if (this.hero.ModelDirection == ModelDirection.Right)
                {
                    this.hero.Flip();
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                this.hero.Position2D = new Vector2(this.hero.Position2D.X + 0.1f, this.hero.Position2D.Y);
                if (this.hero.ModelDirection == ModelDirection.Left)
                {
                    this.hero.Flip();
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                this.hero.Position2D = new Vector2(this.hero.Position2D.X, this.hero.Position2D.Y + 0.1f);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                this.hero.Position2D = new Vector2(this.hero.Position2D.X, this.hero.Position2D.Y - 0.1f);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F12) && this.lastState.IsKeyUp(Keys.F12))
            {
                Globals.displayState = (DISPLAY_STATE)((int)(Globals.displayState + 1) % 3);
            }

            this.lastState = Keyboard.GetState();

            SceneManager.Update(gameTime);

            //AIManager.Update(gameTime);

            // scenemanager.Update(gameTime);
            // TODO: Add your update logic here

            // updates all objects in the components array
            base.Update(gameTime);
        }

        /// <summary>
        /// The setup ai.
        /// </summary>
        //private void SetupAI()
        //{
        //    this.aiManager.objectList = new List<IAIEntity>();

        //    // this.aiManager.objectList.Add(this.monster);
        //}

        /// <summary>
        /// The setup scenes.
        /// </summary>
        private void SetupScenes()
        {
            // TODO: Add your initialization logic here
            /* test stuff starts */

            // this camera follows the hero
            this.heroCamera = new CharacterFollowCamera(this, new Vector3(-10.0f, 0.0f, 20.0f));

            this.hero = new Hero(this) { Position2D = new Vector2(-8.0f, -13.0f) };
            var lollypop = new StaticModel(this)
                {
                    ModelName = "mesh//lollypop", 
                    Position2D = new Vector2(-20.0f, 0.0f), 
                    YawRotation = 0.0f, 
                    PitchRotation = 0.0f, 
                    RollRotation = MathHelper.PiOver4
                };
            var rock = new StaticModel(this) { ModelName = "mesh//rock", Position2D = new Vector2(10.0f, 10.0f) };
            var candy = new StaticModel(this) { ModelName = "mesh//candy", Position2D = new Vector2(-10.0f, 10.0f) };

            var dl1 = new DirectionalLight(this) { Direction = new Vector3(-0.3f, -0.7f, 0.0f), Color = Color.White };

            var scene1 = new MyScene(this) { EnableAI = true, EnableScripts = true, MainGameScene = true };

            // this camera is movable
            this.camera = new MyCamera(this, new Vector3(0.0f, 0.0f, 10.0f));
            //scene1.Camera = this.camera;
            scene1.Camera = heroCamera;
            scene1.LoadContent("levels/test");
            scene1.AddComponent(this.hero);
            this.hero.Position2D = scene1.CurrentLevel.StartPosition;

            // Testing bunny AI
            var bunny = new Bunny(this) { Position2D = new Vector2(-10.0f, -10.0f) };
            var secondBunny = new Bunny(this) { Position2D = new Vector2(-20.0f, 40.0f) };
            this.monster = bunny;

            // bunny.Debug = true;
            //scene1.AIManager.objectList.Add(bunny);
            //scene1.AIManager.objectList.Add(secondBunny);
            scene1.AddComponent(bunny);
            scene1.AddComponent(secondBunny);
            scene1.AddComponent(lollypop);
            scene1.AddComponent(rock);
            scene1.AddComponent(candy);
            scene1.AddComponent(new HeroHealthBar(this) { DrawOrder = int.MaxValue });

            scene1.AddComponent(
                new TutorialText(this)
                    {
                        HeaderText = "Drawing effects", 
                        BodyText = "Use F12 to switch through the\n drawing effects (3D, 2D or Both)", 
                        Position2D = new Vector2(0.0f, 0.0f), 
                        TextBoxSize = new Vector2(30.0f, 25.0f) });

            scene1.Light = dl1;

            scene1.AddComponent(
                new PointLight(this) { Position2D = new Vector2(-10.0f, 0.0f), Radius = 10.0f, Color = Color.White });
            scene1.AddComponent(
                new PointLight(this) { Position2D = new Vector2(10.0f, 0.0f), Radius = 10.0f, Color = Color.Yellow });

            this.IsMouseVisible = true;
            var gameGui = new GameGUI(this) { DrawOrder = int.MaxValue - 1 };
            scene1.AddComponent(gameGui);

            scene1.AddComponent(
                new Bounds(this) { Position2D = new Vector2(0.0f, 30.0f), Size = new Vector2(10.0f, 10.0f) });

            // scene1.GameGUI = new GameGUI(this);
            // scene1.GameGUI.LoadContent();
            // scene1.GameGUI.Scene = scene1;
            // scene1.GameGUIRun = true;);
            this.mainMenu = new MainMenu(this);

            SceneManager.AddScene(this.mainMenu);
            SceneManager.AddScene(scene1);

            SceneManager.ChangeScene(SceneManager.SCENE_ID.MAIN_MENU);

            SceneManager.Initialize();

            /* test stuff ends*/
        }

        #endregion
    }
}