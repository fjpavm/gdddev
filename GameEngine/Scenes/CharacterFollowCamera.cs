// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CharacterFollowCamera.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The character follow camera.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Scenes
{
    using Gdd.Game.Engine.Levels;
    using Gdd.Game.Engine.Levels.Characters;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// The character follow camera.
    /// </summary>
    public class CharacterFollowCamera : Camera
    {
        #region Constants and Fields

        /// <summary>
        /// The last direction.
        /// </summary>
        private ModelDirection lastDirection;

        /// <summary>
        /// The lookat offset.
        /// </summary>
        private Vector3 lookatOffset;

        /// <summary>
        /// The multiplier.
        /// </summary>
        private float multiplier;

        /// <summary>
        /// The position offset.
        /// </summary>
        private Vector3 positionOffset;

        /// <summary>
        /// The target multiplier.
        /// </summary>
        private float targetMultiplier;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterFollowCamera"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        /// <param name="position">
        /// The position.
        /// </param>
        public CharacterFollowCamera(Game game, Vector3 position)
            : base(game, position)
        {
            this.lastDirection = ModelDirection.Right;
            this.multiplier = 1.0f;
            this.targetMultiplier = 1.0f;
            this.lookatOffset = new Vector3(15.0f * this.multiplier, 10.0f, 0.0f);
            this.positionOffset = new Vector3(15.0f * this.multiplier, 15.0f, 35.0f);
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
            this.position = Hero.GetHeroPosition() + this.positionOffset;
            this.lookVector = Hero.GetHeroPosition() + this.lookatOffset;

            this.View = Matrix.CreateLookAt(this.position, this.lookVector, Vector3.Up);

            if (this.lastDirection != Hero.GetHeroDirection())
            {
                this.targetMultiplier *= -1.0f;
                this.lastDirection = Hero.GetHeroDirection();
            }

            if (this.targetMultiplier != this.multiplier)
            {
                this.multiplier += 0.025f * this.targetMultiplier;

                if (this.multiplier > 1.0f)
                {
                    this.multiplier = 1.0f;
                }
                else if (this.multiplier < -1.0f)
                {
                    this.multiplier = -1.0f;
                }

                this.lookatOffset.X = 15.0f * this.multiplier;
                this.positionOffset.X = 15.0f * this.multiplier;
            }
        }

        #endregion
    }
}