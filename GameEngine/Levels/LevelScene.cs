// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelScene.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The level scene.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;

    using Gdd.Game.Engine.Common;
    using Gdd.Game.Engine.Scenes;
    using Gdd.Game.Engine.Scenes.Lights;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The level scene.
    /// </summary>
    public class LevelScene : Scene
    {
        #region Constants and Fields

        /// <summary>
        /// The background.
        /// </summary>
        private readonly Background background;

        /// <summary>
        /// The level entity type bindings.
        /// </summary>
        private static List<LevelEntityTypeBinding> levelEntityTypeBindings;

        /// <summary>
        /// The current level.
        /// </summary>
        private Level currentLevel;

        /// <summary>
        /// The level script.
        /// </summary>
        private LevelScript levelScript;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="LevelScene"/> class.
        /// </summary>
        static LevelScene()
        {
            levelEntityTypeBindings = new List<LevelEntityTypeBinding>();

            foreach (Type subType in typeof(LevelEntity).GetSubTypes())
            {
                var bindingAttribute =
                    subType.GetCustomAttributes(typeof(LevelEntityBindingAttribute), false).FirstOrDefault() as
                    LevelEntityBindingAttribute;
                if (bindingAttribute == null)
                {
                    continue;
                }

                Type sceneComponentType = Type.GetType(bindingAttribute.ClassName);
                var binding = new LevelEntityTypeBinding(subType, sceneComponentType);
                levelEntityTypeBindings.Add(binding);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelScene"/> class.
        /// </summary>
        /// <param name="game">
        /// The game instance.
        /// </param>
        public LevelScene(Game game)
            : base(game)
        {
            this.currentLevel = new Level(true);
            ObjectManager.SetUpLists(this.ID);
            this.background = new Background(this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets LevelEntityTypeBindings.
        /// </summary>
        public static IEnumerable<LevelEntityTypeBinding> LevelEntityTypeBindings
        {
            get
            {
                return levelEntityTypeBindings;
            }
        }

        /// <summary>
        /// Gets or sets CurrentLevel.
        /// </summary>
        public Level CurrentLevel
        {
            get
            {
                return this.currentLevel;
            }

            set
            {
                if (value != null)
                {
                    this.ClearComponents();
                    foreach (DrawableSceneComponent component in value.Components)
                    {
                        component.SetGame(this.Game);
                        this.AddComponent(component);
                    }

                    value.Components.AutoInitialize = true;

                    this.currentLevel = value;
                    this.Initialize();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether EnableScripts.
        /// </summary>
        public bool EnableScripts { get; set; }

        /// <summary>
        /// Gets LevelScript.
        /// </summary>
        public LevelScript LevelScript
        {
            get
            {
                return this.levelScript;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The add component.
        /// </summary>
        /// <param name="sceneComponent">
        /// The scene component.
        /// </param>
        public override void AddComponent(SceneComponent sceneComponent)
        {
            base.AddComponent(sceneComponent);
            this.currentLevel.Components.Add(sceneComponent);
        }

        /// <summary>
        /// The initialize.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            if (this.Camera == null)
            {
                this.Camera = new Camera(this.Game, new Vector3(0.0f, 0.0f, 30.0f)) { FieldOfView = 70.0f };
            }

            if (this.Light == null)
            {
                this.Light = new DirectionalLight(this.Game)
                    {
                       Position3D = new Vector3(0.0f, 0.0f, 10.0f), Color = Color.CornflowerBlue 
                    };
            }

            this.levelScript = Script.Load(this.currentLevel.Script) as LevelScript;
            if (this.EnableScripts && this.levelScript != null)
            {
                this.levelScript.Level = this.currentLevel;
                this.levelScript.Initialize();
            }
        }

        /// <summary>
        /// The load content.
        /// </summary>
        /// <param name="levelName">
        /// The level name.
        /// </param>
        public void LoadContent(string levelName)
        {
            this.CurrentLevel = this.Game.Content.Load<Level>(levelName);
            this.background.LoadContent();
        }

        /// <summary>
        /// The load level.
        /// </summary>
        /// <param name="filename">
        /// The file name.
        /// </param>
        public void LoadFile(string filename)
        {
            using (var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var xmlSerializer = new XmlSerializer(
                    typeof(Level), typeof(DrawableSceneComponent).GetSubTypes().ToArray());
                this.CurrentLevel = xmlSerializer.Deserialize(fileStream) as Level;
            }
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.EnableScripts && this.levelScript != null)
            {
                this.levelScript.Update(gameTime);
            }

            foreach (SceneComponent block in this.currentLevel.Components)
            {
                block.Update(gameTime);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The draw background.
        /// </summary>
        protected override void DrawBackground()
        {
            IEnumerable<Ground> groundObjects = from dsc in this.DrawableSceneComponents
                                                where dsc is Ground
                                                let g = (Ground)dsc
                                                select g;
            if (groundObjects.Count() != 0)
            {
                this.background.SetBounds(
                    groundObjects.Min(g => g.PhysicsGeometry.AABB.Min.X), 
                    groundObjects.Max(g => g.PhysicsGeometry.AABB.Max.X));
            }
            else
            {
                // this.background.SetBounds();                
            }

            this.background.Draw();
        }

        #endregion
    }
}