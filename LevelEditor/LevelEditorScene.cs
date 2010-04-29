// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelEditorScene.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The level editor scene.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.LevelEditor
{
    using Gdd.Game.Engine;
    using Gdd.Game.Engine.Input;
    using Gdd.Game.Engine.Levels;
    using Gdd.Game.Engine.Scenes.Lights;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The level editor scene.
    /// </summary>
    internal class LevelEditorScene : LevelScene
    {
        #region Constants and Fields

        /// <summary>
        /// The camera down.
        /// </summary>
        private GameAction cameraDown;

        /// <summary>
        /// The camera left.
        /// </summary>
        private GameAction cameraLeft;

        /// <summary>
        /// The camera right.
        /// </summary>
        private GameAction cameraRight;

        /// <summary>
        /// The camera up.
        /// </summary>
        private GameAction cameraUp;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelEditorScene"/> class.
        /// </summary>
        /// <param name="game">
        /// The Game instance.
        /// </param>
        public LevelEditorScene(Game game)
            : base(game)
        {
            this.InputManager = new InputManager();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The initialize.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.Camera = new Camera(this.Game, new Vector3(0.0f, 0.0f, 30.0f)) { FieldOfView = 70.0f };
            this.Light = new DirectionalLight(this.Game)
                {
                   Position3D = new Vector3(0.0f, 0.0f, 10.0f), Color = Color.CornflowerBlue 
                };

            this.cameraUp = new GameAction("cameraUp");
            this.InputManager.MapToKey(this.cameraUp, new[] { Keys.Up, Keys.LeftControl });
            this.cameraDown = new GameAction("cameraDown");
            this.InputManager.MapToKey(this.cameraDown, new[] { Keys.Down, Keys.LeftControl });
            this.cameraRight = new GameAction("cameraRight");
            this.InputManager.MapToKey(this.cameraRight, new[] { Keys.Right, Keys.LeftControl });
            this.cameraLeft = new GameAction("cameraLeft");
            this.InputManager.MapToKey(this.cameraLeft, new[] { Keys.Left, Keys.LeftControl });
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
            const float Delta = 0.1f;
            if (this.cameraDown.IsPressed)
            {
                this.Camera.MoveUpDown(-Delta);
            }

            if (this.cameraLeft.IsPressed)
            {
                this.Camera.StrafeRightLeft(-Delta);
            }

            if (this.cameraRight.IsPressed)
            {
                this.Camera.StrafeRightLeft(Delta);
            }

            if (this.cameraUp.IsPressed)
            {
                this.Camera.MoveUpDown(Delta);
            }
        }

        #endregion
    }
}