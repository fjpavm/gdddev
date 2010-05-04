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
    using System;

    using Gdd.Game.Engine.Input;
    using Gdd.Game.Engine.Levels;
    using Gdd.Game.Engine.Scenes.Lights;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The level editor scene.
    /// </summary>
    internal sealed class LevelEditorScene : LevelScene
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
            this.EnablePhysics = false;
        }

        #endregion

        #region Events

        /// <summary>
        /// The camera position changed.
        /// </summary>
        public event EventHandler<CameraPositionChangedEventArgs> CameraPositionChanged;

        #endregion

        #region Public Methods

        /// <summary>
        /// The initialize.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            var newPosition = new Vector2(this.Camera.Pos.X, this.Camera.Pos.Y);
            this.OnCameraPositionChanged(new CameraPositionChangedEventArgs(newPosition));
            this.Light = new DirectionalLight(this.Game)
                {
                   Position3D = new Vector3(0.0f, 0.0f, 10.0f), Color = Color.CornflowerBlue 
                };

            this.cameraUp = new GameAction("cameraUp");
            this.InputManager.MapToKey(this.cameraUp, Keys.Up);
            this.cameraDown = new GameAction("cameraDown");
            this.InputManager.MapToKey(this.cameraDown, Keys.Down);
            this.cameraRight = new GameAction("cameraRight");
            this.InputManager.MapToKey(this.cameraRight, Keys.Right);
            this.cameraLeft = new GameAction("cameraLeft");
            this.InputManager.MapToKey(this.cameraLeft, Keys.Left);
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
            bool cameraPositionChanged = false;
            if (this.cameraDown.IsPressed)
            {
                this.Camera.MoveUpDown(-Delta);
                cameraPositionChanged = true;
            }

            if (this.cameraLeft.IsPressed)
            {
                this.Camera.StrafeRightLeft(-Delta);
                cameraPositionChanged = true;
            }

            if (this.cameraRight.IsPressed)
            {
                this.Camera.StrafeRightLeft(Delta);
                cameraPositionChanged = true;
            }

            if (this.cameraUp.IsPressed)
            {
                this.Camera.MoveUpDown(Delta);
                cameraPositionChanged = true;
            }

            if (cameraPositionChanged)
            {
                var cameraPosition = new Vector2(this.Camera.Pos.X, this.Camera.Pos.Y);
                this.OnCameraPositionChanged(new CameraPositionChangedEventArgs(cameraPosition));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The invoke camera position changed.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        private void OnCameraPositionChanged(CameraPositionChangedEventArgs e)
        {
            EventHandler<CameraPositionChangedEventArgs> handler = this.CameraPositionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion
    }
}