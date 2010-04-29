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
    using System.Linq;
    using System.Xml.Serialization;

    using Gdd.Game.Engine.Levels;
    using Gdd.Game.Engine.Scenes;

    using Microsoft.Xna.Framework.Content.Pipeline;

    /// <summary>
    /// The level processor.
    /// </summary>
    [ContentProcessor(DisplayName = "Level Processor")]
    internal class LevelProcessor : ContentProcessor<byte[], Level>
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
        public override Level Process(byte[] input, ContentProcessorContext context)
        {
            using (var memoryStream = new MemoryStream(input))
            {
                var xmlSerializer = new XmlSerializer(
                    typeof(Level), typeof(DrawableSceneComponent).GetSubTypes().ToArray());
                return xmlSerializer.Deserialize(memoryStream) as Level;
            }
        }

        #endregion
    }
}