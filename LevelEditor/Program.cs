// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.LevelEditor
{
    using System;
    using System.Windows.Forms;

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
        /// The command-line args.
        /// </param>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            using (var form = new LevelEditorForm())
            {
                form.Show();
                using (var game = new LevelEditorPane(form.GetDrawSurface()))
                {
                    form.SetLevelEditorPane(game);
                    game.Run();
                }
            }
        }

        #endregion
    }
}