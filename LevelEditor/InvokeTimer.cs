// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvokeTimer.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The invoke timer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.LevelEditor
{
    using System.Reflection;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// The invoke timer.
    /// </summary>
    internal sealed class InvokeTimer : GameComponent
    {
        #region Constants and Fields

        /// <summary>
        /// The invoke parameters.
        /// </summary>
        private object[] invokeParameters;

        /// <summary>
        /// The invoke target.
        /// </summary>
        private bool invokeTarget;

        /// <summary>
        /// The timer.
        /// </summary>
        private double timer;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InvokeTimer"/> class.
        /// </summary>
        /// <param name="game">
        /// The game.
        /// </param>
        public InvokeTimer(Game game)
            : base(game)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Interval.
        /// </summary>
        public double Interval { get; set; }

        /// <summary>
        /// Gets or sets Target.
        /// </summary>
        public object Target { get; set; }

        /// <summary>
        /// Gets or sets TargetMethod.
        /// </summary>
        public MethodInfo TargetMethod { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The invoke.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        public void Invoke(params object[] parameters)
        {
            this.invokeTarget = true;
            this.invokeParameters = parameters;
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

            if (this.timer > 0)
            {
                this.timer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            if (!this.invokeTarget || this.timer > 0)
            {
                return;
            }

            this.invokeTarget = false;
            this.timer = this.Interval;
            this.TargetMethod.Invoke(this.Target, this.invokeParameters);
        }

        #endregion
    }
}