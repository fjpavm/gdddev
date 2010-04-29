// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputManager.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The input manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Input
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// The input manager.
    /// </summary>
    public class InputManager
    {
        #region Constants and Fields

        /// <summary>
        /// The key actions.
        /// </summary>
        private readonly Dictionary<Keys[], GameAction> keyActions;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InputManager"/> class.
        /// </summary>
        public InputManager()
        {
            // TODO: implement mapping game actions to mouse events
            this.keyActions = new Dictionary<Keys[], GameAction>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The clear map.
        /// </summary>
        /// <param name="action">
        /// The action.
        /// </param>
        public void ClearMap(GameAction action)
        {
            foreach (var keyAction in this.keyActions)
            {
                if (keyAction.Value == action)
                {
                    this.keyActions.Remove(keyAction.Key);
                }
            }
        }

        /// <summary>
        /// The map to key.
        /// </summary>
        /// <param name="action">
        /// Game action.
        /// </param>
        /// <param name="keys">
        /// The keys that will trigger the action.
        /// </param>
        public void MapToKey(GameAction action, Keys[] keys)
        {
            if (keys == null || keys.Length == 0)
            {
                return;
            }

            if (this.keyActions.ContainsKey(keys))
            {
                this.keyActions[keys] = action;
            }
            else
            {
                this.keyActions.Add(keys, action);
            }
        }

        /// <summary>
        /// The map to key.
        /// </summary>
        /// <param name="action">
        /// Game action.
        /// </param>
        /// <param name="key">
        /// The key that will trigger the action.
        /// </param>
        public void MapToKey(GameAction action, Keys key)
        {
            this.MapToKey(action, new[] { key });
        }

        /// <summary>
        /// The reset all game actions.
        /// </summary>
        public void ResetAllGameActions()
        {
            this.keyActions.Clear();
        }

        /// <summary>
        /// The update.
        /// </summary>
        public void Update()
        {
            foreach (var keyAction in this.keyActions)
            {
                bool isPressed = false;
                foreach (Keys key in keyAction.Key)
                {
                    if (Keyboard.GetState().IsKeyDown(key))
                    {
                        isPressed = true;
                    }
                    else
                    {
                        isPressed = false;
                        break;
                    }
                }

                if (isPressed)
                {
                    keyAction.Value.Press();
                }
                else
                {
                    keyAction.Value.Release();
                }
            }
        }

        #endregion
    }
}