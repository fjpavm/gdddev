// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelProcessor.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The level processor.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Content.Pipeline.Processors
{
    using System.IO;

    using Gdd.Game.Engine.Levels;

    using Microsoft.Xna.Framework.Content.Pipeline;

    /// <summary>
    /// The level processor.
    /// </summary>
    [ContentProcessor(DisplayName = "Level Processor - GddGame")]
    internal class LevelProcessor : ContentProcessor<byte[], SerializableLevel>
    {
        #region Public Methods

        /// <summary>
        /// Processes the specified input data and returns the result.
        /// </summary>
        /// <param name="input">
        /// The input.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// Returns the Level instance.
        /// </returns>
        public override SerializableLevel Process(byte[] input, ContentProcessorContext context)
        {
            using (var memoryStream = new MemoryStream(input))
            {
                var levelSerializer = new LevelSerializer();
                return levelSerializer.Deserialize(memoryStream);
            }
        }

        #endregion
    }
}