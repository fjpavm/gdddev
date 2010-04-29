// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MyCamera.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The my camera.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game
{
    using Engine;
    using Engine.Input;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The my camera.
    /// </summary>
    public class MyCamera : Camera
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MyCamera"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        /// <param name="pos">
        /// The pos.
        /// </param>
        public MyCamera(Game game, Vector3 pos)
            : base(game, pos)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="gameTime">
        /// The game time.
        /// </param>
        public override void Update(GameTime gameTime)
        {
            Actions.InputManager.Update();

            const float Delta = 0.3f;

            if (Actions.CameraMoveBackward.IsPressed && !Actions.CameraMoveDown.IsPressed)
            {
                this.MoveForwardBackward(-Delta);
            }
            else if (Actions.CameraMoveDown.IsPressed)
            {
                this.MoveUpDown(-Delta);
            }

            if (Actions.CameraMoveForward.IsPressed && !Actions.CameraMoveUp.IsPressed)
            {
                this.MoveForwardBackward(Delta);
            }
            else if (Actions.CameraMoveUp.IsPressed)
            {
                this.MoveUpDown(Delta);
            }

            if (Actions.CameraTurnLeft.IsPressed && !Actions.CameraStrafeLeft.IsPressed)
            {
                this.Yaw(-Delta);
            }
            else if (Actions.CameraStrafeLeft.IsPressed)
            {
                this.StrafeRightLeft(-Delta);
            }

            if (Actions.CameraTurnRight.IsPressed && !Actions.CameraStrafeRight.IsPressed)
            {
                this.Yaw(Delta);
            }
            else if (Actions.CameraStrafeRight.IsPressed)
            {
                this.StrafeRightLeft(Delta);
            }

            base.Update(gameTime);
        }

        #endregion

        /// <summary>
        /// The actions.
        /// </summary>
        private static class Actions
        {
            #region Constructors and Destructors

            /// <summary>
            /// Initializes static members of the <see cref="Actions"/> class.
            /// </summary>
            static Actions()
            {
                InputManager = new InputManager();
                CameraMoveBackward = new GameAction("cameraMoveBackward");
                InputManager.MapToKey(CameraMoveBackward, Keys.Down);
                CameraMoveDown = new GameAction("cameraMoveDown");
                InputManager.MapToKey(CameraMoveDown, new[] { Keys.Down, Keys.LeftShift });
                CameraMoveForward = new GameAction("cameraMoveForward");
                InputManager.MapToKey(CameraMoveForward, Keys.Up);
                CameraMoveUp = new GameAction("cameraMoveUp");
                InputManager.MapToKey(CameraMoveUp, new[] { Keys.Up, Keys.LeftShift });
                CameraStrafeLeft = new GameAction("cameraStrafeLeft");
                InputManager.MapToKey(CameraStrafeLeft, new[] { Keys.Left, Keys.LeftShift });
                CameraStrafeRight = new GameAction("cameraStrafeRight");
                InputManager.MapToKey(CameraStrafeRight, new[] { Keys.Right, Keys.LeftShift });
                CameraTurnLeft = new GameAction("cameraTurnLeft");
                InputManager.MapToKey(CameraTurnLeft, Keys.Left);
                CameraTurnRight = new GameAction("cameraTurnRight");
                InputManager.MapToKey(CameraTurnRight, Keys.Right);
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets CameraMoveBackward.
            /// </summary>
            public static GameAction CameraMoveBackward { get; private set; }

            /// <summary>
            /// Gets CameraMoveDown.
            /// </summary>
            public static GameAction CameraMoveDown { get; private set; }

            /// <summary>
            /// Gets CameraMoveForward.
            /// </summary>
            public static GameAction CameraMoveForward { get; private set; }

            /// <summary>
            /// Gets CameraMoveUp.
            /// </summary>
            public static GameAction CameraMoveUp { get; private set; }

            /// <summary>
            /// Gets CameraStrafeLeft.
            /// </summary>
            public static GameAction CameraStrafeLeft { get; private set; }

            /// <summary>
            /// Gets CameraStrafeRight.
            /// </summary>
            public static GameAction CameraStrafeRight { get; private set; }

            /// <summary>
            /// Gets CameraTurnLeft.
            /// </summary>
            public static GameAction CameraTurnLeft { get; private set; }

            /// <summary>
            /// Gets CameraTurnRight.
            /// </summary>
            public static GameAction CameraTurnRight { get; private set; }

            /// <summary>
            /// Gets InputManager.
            /// </summary>
            public static InputManager InputManager { get; private set; }

            #endregion
        }
    }
}