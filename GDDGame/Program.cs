// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game
{
    /// <summary>
    /// The program.
    /// </summary>
    internal static class Program
    {
        #region Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        private static void Main(string[] args)
        {
            using (var game = new Game1())
            {
                game.Run();
            }
        }

        #endregion
    }
}