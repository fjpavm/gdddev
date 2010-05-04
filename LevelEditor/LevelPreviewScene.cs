// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelPreviewScene.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The level preview scene.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.LevelEditor
{
    using Gdd.Game.Engine.Input;
    using Gdd.Game.Engine.Levels;
    using Gdd.Game.Engine.Levels.Characters;
    using Gdd.Game.Engine.Scenes;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The level preview scene.
    /// </summary>
    internal sealed class LevelPreviewScene : LevelScene
    {
        #region Constants and Fields

        /// <summary>
        /// The hero.
        /// </summary>
        private readonly Hero hero;

        /// <summary>
        /// The action jump.
        /// </summary>
        private GameAction actionJump;

        /// <summary>
        /// The action move down.
        /// </summary>
        private GameAction actionMoveDown;

        /// <summary>
        /// The action move left.
        /// </summary>
        private GameAction actionMoveLeft;

        /// <summary>
        /// The action move right.
        /// </summary>
        private GameAction actionMoveRight;

        /// <summary>
        /// The action move up.
        /// </summary>
        private GameAction actionMoveUp;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LevelPreviewScene"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public LevelPreviewScene(Game game)
            : base(game)
        {
            this.hero = new Hero(this.Game);
            this.InputManager = new InputManager();
            this.Camera = new CharacterFollowCamera(this.Game, Vector3.Zero);
            this.AddComponent(this.hero);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The initialize.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();
            this.actionJump = new GameAction("jump", GameActionBehavior.Normal);
            this.InputManager.MapToKey(this.actionJump, Keys.Space);
            this.actionMoveDown = new GameAction("moveDown", GameActionBehavior.Normal);
            this.InputManager.MapToKey(this.actionMoveDown, Keys.S);
            this.actionMoveLeft = new GameAction("moveLeft", GameActionBehavior.Normal);
            this.InputManager.MapToKey(this.actionMoveLeft, Keys.A);
            this.actionMoveRight = new GameAction("moveRight", GameActionBehavior.Normal);
            this.InputManager.MapToKey(this.actionMoveRight, Keys.D);
            this.actionMoveUp = new GameAction("moveUp", GameActionBehavior.Normal);
            this.InputManager.MapToKey(this.actionMoveUp, Keys.W);
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

            if (this.actionJump.IsPressed)
            {
            }
            else if (this.actionMoveDown.IsPressed)
            {
                this.hero.Position2D = new Vector2(this.hero.Position2D.X, this.hero.Position2D.Y - 0.1f);
            }
            else if (this.actionMoveLeft.IsPressed)
            {
                this.hero.Position2D = new Vector2(this.hero.Position2D.X - 0.1f, this.hero.Position2D.Y);
                if (this.hero.ModelDirection == ModelDirection.Right)
                {
                    this.hero.Flip();
                }
            }
            else if (this.actionMoveRight.IsPressed)
            {
                this.hero.Position2D = new Vector2(this.hero.Position2D.X + 0.1f, this.hero.Position2D.Y);
                if (this.hero.ModelDirection == ModelDirection.Left)
                {
                    this.hero.Flip();
                }
            }
            else if (this.actionMoveUp.IsPressed)
            {
                this.hero.Position2D = new Vector2(this.hero.Position2D.X, this.hero.Position2D.Y + 0.1f);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on current level changed.
        /// </summary>
        protected override void OnCurrentLevelChanged()
        {
            base.OnCurrentLevelChanged();
            this.hero.Position2D = this.CurrentLevel.StartPosition;
            this.AddComponent(this.hero);
        }

        #endregion
    }
}