// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelImporter.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The level importer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Content.Pipeline
{
    using System.IO;

    using Microsoft.Xna.Framework.Content.Pipeline;

    /// <summary>
    /// The level importer.
    /// </summary>
    [ContentImporter(".lvl", DisplayName = "Level Importer", DefaultProcessor = "LevelProcessor")]
    public class LevelImporter : ContentImporter<byte[]>
    {
        #region Public Methods

        /// <summary>
        /// The import.
        /// </summary>
        /// <param name="filename">
        /// Name of a game asset file.
        /// </param>
        /// <param name="context">
        /// Contains information for importing a game asset, such as a logger interface.
        /// </param>
        /// <returns>
        /// Returns serialized level file.
        /// </returns>
        public override byte[] Import(string filename, ContentImporterContext context)
        {
            using (var fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[fileStream.Length];
                fileStream.Read(buffer, 0, (int)fileStream.Length);
                return buffer;
            }
        }

        #endregion
    }
}