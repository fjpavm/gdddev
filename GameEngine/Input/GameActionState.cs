// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameActionState.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The game action state.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Input
{
    /// <summary>
    /// The game action state.
    /// </summary>
    public enum GameActionState
    {
        /// <summary>
        /// The released.
        /// </summary>
        Released, 

        /// <summary>
        /// The pressed.
        /// </summary>
        Pressed, 

        /// <summary>
        /// The waiting for release.
        /// </summary>
        WaitingForRelease
    }
}