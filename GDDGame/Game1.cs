using Gdd.Game.Engine.Levels.Characters;
using Gdd.Game.Engine.Scenes;
using Gdd.Game.Engine.Levels;
using Gdd.Game.Engine.Levels.Information;
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
    using System;
    using System.Collections.Generic;

    using Engine;
    using Engine.AI;
    using Engine.AI.StateMachines;
    using Engine.Levels;
    using Engine.Levels.Characters;
    using Engine.Menu;
    using Engine.Physics;
    using Engine.Render.Shadow;
    using Engine.Scenes;
    using Engine.Scenes.Lights;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;
    using Gdd.Game.Engine.GUI;

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        #region Constants and Fields

        /// <summary>
        /// The aiManager
        /// </summary>
        private AIManager aiManager;

        /// <summary>
        /// The camera.
        /// </summary>
        private CharacterFollowCamera heroCamera;

        private MyCamera camera;

        /// <summary>
        /// The graphics.
        /// </summary>
        private GraphicsDeviceManager graphics;

        /// <summary>
        /// The hero.
        /// </summary>
        private Hero hero;

        // FrankM: Just for testing;
        /// <summary>
        /// The monster.
        /// </summary>
        private AIMonster monster;

        private ScenePhysics physics;

        /// <summary>
        /// The sprite batch.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// The main menu.
        /// </summary>
        private MainMenu mainMenu;

        #endregion

        // private Character character1;
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Game1"/> class.
        /// </summary>
        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.aiManager = new AIManager(this);
            this.Content.RootDirectory = "Content";

            // add the GFXComponent into the components
            this.Components.Add(new GfxComponent(this, graphics));

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

            this.SetupAI();
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

            SpriteFont headerFont = Content.Load<SpriteFont>("Font//TutorialHeader");
            SpriteFont bodyFont = Content.Load<SpriteFont>("Font//TutorialBody");

            TutorialText.SetFonts(headerFont, bodyFont);
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
            Message m = null;

            // Console.WriteLine("well well well well well well well well well" + character1.Position2D);
            // character1 = new Character(this, "mesh//hero03");
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                SceneManager.ChangeScene(SceneManager.SCENE_ID.IN_GAME_MENU);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                m = new Message();
                m.MessageType = MessageTypes.die;
                m.timeDelivery = gameTime.TotalGameTime.TotalSeconds;
                m.to = this.monster;
                AIManager.messageQueue.sendMessage(m);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Z))
            {
                m = new Message();
                m.MessageType = MessageTypes.resurect;
                m.timeDelivery = gameTime.TotalGameTime.TotalSeconds;
                m.to = this.monster;
                AIManager.messageQueue.sendMessage(m);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                this.hero.Position2D = new Vector2(this.hero.Position2D.X - 0.1f, this.hero.Position2D.Y);
                if (this.hero.ModelDirection == ModelDirection.Right)
                    this.hero.Flip();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                this.hero.Position2D = new Vector2(this.hero.Position2D.X + 0.1f, this.hero.Position2D.Y);
                if (this.hero.ModelDirection == ModelDirection.Left)
                    this.hero.Flip();
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                this.hero.Position2D = new Vector2(this.hero.Position2D.X, this.hero.Position2D.Y + 0.1f);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                this.hero.Position2D = new Vector2(this.hero.Position2D.X, this.hero.Position2D.Y - 0.1f);
            }

            SceneManager.Update(gameTime);

            this.aiManager.Update(gameTime);

            // scenemanager.Update(gameTime);
            // TODO: Add your update logic here

            // updates all objects in the components array
            base.Update(gameTime);
        }

        /// <summary>
        /// The setup ai.
        /// </summary>
        private void SetupAI()
        {

            this.aiManager.objectList = new List<IAIEntity>();
           // this.aiManager.objectList.Add(this.monster);
   
        }

        /// <summary>
        /// The setup scenes.
        /// </summary>
        private void SetupScenes()
        {
            // TODO: Add your initialization logic here
            /* test stuff starts */

            // this camera follows the hero
            this.heroCamera = new CharacterFollowCamera(this, new Vector3(-10.0f, 0.0f, 20.0f));

            // this camera is movable
            camera = new MyCamera(this, new Vector3(0.0f, 0.0f, 10.0f));

            this.hero = new Hero(this) { Position2D = new Vector2(-8.0f, -13.0f) };
            StaticModel lollypop = new StaticModel(this) { ModelName = "mesh//lollypop", Position2D = new Vector2(-20.0f, 0.0f), YawRotation = 0.0f, PitchRotation = 0.0f, RollRotation = MathHelper.PiOver4};
            StaticModel rock = new StaticModel(this) { ModelName = "mesh//rock", Position2D = new Vector2(10.0f, 10.0f) };
            StaticModel candy = new StaticModel(this) { ModelName = "mesh//candy", Position2D = new Vector2(-10.0f, 10.0f) };


            var dl1 = new DirectionalLight(this) { Direction = new Vector3(-0.3f, -0.7f, 0.0f), Color = Color.White };
            var dl2 = new DirectionalLight(this) { Direction = new Vector3(-0.5f, -0.5f, 0.0f), Color = Color.Blue };

            var scene1 = new MyScene(this) { EnableScripts = false, Visible = false, MainGameScene = true };
            scene1.LoadContent("levels/test");
            scene1.AddComponent(this.hero);
            // Testing bunny AI
            Bunny bunny = new Bunny(this) { Position2D = new Vector2(-10.0f, -10.0f) };
            this.monster = bunny;

            aiManager.objectList.Add(bunny);
            scene1.AddComponent(bunny);
            scene1.AddComponent(lollypop);
            scene1.AddComponent(rock);
            scene1.AddComponent(candy);
            scene1.AddComponent(new HeroHealthBar(this));

            scene1.AddComponent(new TutorialText(this, new Vector2(30.0f, 25.0f)) { HeaderText = "Disable normal drawing", BodyText = "Put SHOW_ONLY_PHYSICS into the \n conditional compilation symbols \n To disable normal drawing", Position2D = new Vector2(0.0f, 0.0f) });
            scene1.AddComponent(new TutorialText(this, new Vector2(15.0f, 15.0f)) { HeaderText = "Enable physics drawing", BodyText = "Put SHOW_PHYSICS into the \n conditional compilation symbols \n To enable physics drawing", Position2D = new Vector2(5.0f, 10.0f) });
            scene1.Camera = camera;
            scene1.Light = dl1;

            scene1.AddComponent(new PointLight(this) { Position2D = new Vector2(-10.0f, 0.0f), Radius = 10.0f, Color = Color.White });
            scene1.AddComponent(new PointLight(this) { Position2D = new Vector2(10.0f, 0.0f), Radius = 10.0f, Color = Color.Yellow });

            this.IsMouseVisible = true;
            scene1.GameGUI = new GameGUI(this);
            scene1.GameGUI.LoadContent();
            scene1.GameGUI.Scene = scene1;
            scene1.GameGUIRun = true;
            
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