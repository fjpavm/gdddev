// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelWriter.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The level writer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Content.Pipeline.Serialization.Compiler
{
    using System.IO;

    using Gdd.Game.Engine.Levels;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

    /// <summary>
    /// The level writer.
    /// </summary>
    [ContentTypeWriter]
    internal class LevelWriter : ContentTypeWriter<SerializableLevel>
    {
        #region Public Methods

        /// <summary>
        /// Gets the assembly qualified name of the runtime loader for this type.
        /// </summary>
        /// <param name="targetPlatform">
        /// Name of the platform.
        /// </param>
        /// <returns>
        /// Returns the assembly qualified name of the runtime loader for this type.
        /// </returns>
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Gdd.Game.Engine.Levels.LevelReader, Gdd.Game.Engine";
        }

        #endregion

        #region Methods

        /// <summary>
        /// The write.
        /// </summary>
        /// <param name="output">
        /// The output.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        protected override void Write(ContentWriter output, SerializableLevel value)
        {
            using (var memoryStream = new MemoryStream())
            {
                var levelSerializer = new LevelSerializer();
                levelSerializer.Serialize(memoryStream, value);
                output.Write(memoryStream.ToArray());
            }
        }

        #endregion
    }
}