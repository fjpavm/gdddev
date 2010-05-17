// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameAction.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The game action.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Input
{
    /// <summary>
    /// The game action.
    /// </summary>
    public class GameAction
    {
        #region Constants and Fields

        /// <summary>
        /// The behavior.
        /// </summary>
        private readonly GameActionBehavior behavior;

        /// <summary>
        /// Action name.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The amount.
        /// </summary>
        private int amount;

        /// <summary>
        /// The state.
        /// </summary>
        private GameActionState state;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GameAction"/> class.
        /// </summary>
        /// <param name="name">
        /// Action name.
        /// </param>
        public GameAction(string name)
            : this(name, GameActionBehavior.Normal)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameAction"/> class.
        /// </summary>
        /// <param name="name">
        /// Action name.
        /// </param>
        /// <param name="behavior">
        /// Action behavior.
        /// </param>
        public GameAction(string name, GameActionBehavior behavior)
        {
            this.name = name;
            this.behavior = behavior;
            this.Reset();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets Amount.
        /// </summary>
        public int Amount
        {
            get
            {
                int retVal = this.amount;
                if (retVal != 0)
                {
                    if (this.state == GameActionState.Released)
                    {
                        this.amount = 0;
                    }
                    else if (this.behavior == GameActionBehavior.DetectInitialPressOnly)
                    {
                        this.state = GameActionState.WaitingForRelease;
                        this.amount = 0;
                    }
                }

                return retVal;
            }
        }

        /// <summary>
        /// Gets a value indicating whether IsPressed.
        /// </summary>
        public bool IsPressed
        {
            get
            {
                return this.Amount != 0;
            }
        }

        /// <summary>
        /// Gets Name.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The press.
        /// </summary>
        public void Press()
        {
            this.Press(1);
        }

        /// <summary>
        /// The press.
        /// </summary>
        /// <param name="keyPressAmount">
        /// The key press amount.
        /// </param>
        public void Press(int keyPressAmount)
        {
            if (this.state != GameActionState.WaitingForRelease)
            {
                this.amount += keyPressAmount;
                this.state = GameActionState.Pressed;
            }
        }

        /// <summary>
        /// The release.
        /// </summary>
        public void Release()
        {
            this.state = GameActionState.Released;
        }

        /// <summary>
        /// The reset.
        /// </summary>
        public void Reset()
        {
            this.state = GameActionState.Released;
            this.amount = 0;
        }

        /// <summary>
        /// Simulate a single press and release.
        /// </summary>
        public void Tap()
        {
            this.Press();
            this.Release();
        }

        #endregion
    }
}