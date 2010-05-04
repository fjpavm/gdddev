// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MyScene.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The my scene.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game
{
    using Gdd.Game.Engine.Input;
    using Gdd.Game.Engine.Levels;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The my scene.
    /// </summary>
    public class MyScene : LevelScene
    {
        #region Constants and Fields

        /// <summary>
        /// The action light move down.
        /// </summary>
        private GameAction actionLightMoveDown;

        /// <summary>
        /// The action light move left.
        /// </summary>
        private GameAction actionLightMoveLeft;

        /// <summary>
        /// The action light move right.
        /// </summary>
        private GameAction actionLightMoveRight;

        /// <summary>
        /// The action light move up.
        /// </summary>
        private GameAction actionLightMoveUp;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MyScene"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public MyScene(Game game)
            : base(game)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The initialize.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.InputManager = new InputManager();
            this.actionLightMoveUp = new GameAction("lightMoveUp");
            this.InputManager.MapToKey(this.actionLightMoveUp, Keys.W);
            this.actionLightMoveDown = new GameAction("lightMoveDown");
            this.InputManager.MapToKey(this.actionLightMoveDown, Keys.S);
            this.actionLightMoveRight = new GameAction("lightMoveRight");
            this.InputManager.MapToKey(this.actionLightMoveRight, Keys.D);
            this.actionLightMoveLeft = new GameAction("lightMoveLeft");
            this.InputManager.MapToKey(this.actionLightMoveLeft, Keys.A);
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
            if (this.directionalLight != null)
            {
                /* if (this.actionLightMoveUp.IsPressed)
                {
                    this.directionalLight.Position = new Vector3(
                        this.directionalLight.Position.X, 
                        this.directionalLight.Position.Y + 1, 
                        this.directionalLight.Position.Z);
                }
                else if (this.actionLightMoveDown.IsPressed)
                {
                    this.directionalLight.Position = new Vector3(
                        this.directionalLight.Position.X, 
                        this.directionalLight.Position.Y - 1, 
                        this.directionalLight.Position.Z);
                }
                else if (this.actionLightMoveLeft.IsPressed)
                {
                    this.directionalLight.Position = new Vector3(
                        this.directionalLight.Position.X - 1, 
                        this.directionalLight.Position.Y, 
                        this.directionalLight.Position.Z);
                }
                else if (this.actionLightMoveRight.IsPressed)
                {
                    this.directionalLight.Position = new Vector3(
                        this.directionalLight.Position.X + 1, 
                        this.directionalLight.Position.Y, 
                        this.directionalLight.Position.Z);
                }
                else 
                if (this.actionLightMoveForward.IsPressed)
                {
                    this.directionalLight.Position3D = new Vector3(
                        this.directionalLight.Position3D.X, 
                        this.directionalLight.Position3D.Y, 
                        this.directionalLight.Position3D.Z + 1);
                }
                else if (this.actionLightMoveBackward.IsPressed)
                {
                    this.directionalLight.Position3D = new Vector3(
                        this.directionalLight.Position3D.X, 
                        this.directionalLight.Position3D.Y, 
                        this.directionalLight.Position3D.Z - 1);
                }*/
            }
        }

        #endregion
    }
}