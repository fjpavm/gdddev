// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelReader.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The level reader.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using Microsoft.Xna.Framework.Content;

    /// <summary>
    /// The level reader.
    /// </summary>
    internal class LevelReader : ContentTypeReader<SerializableLevel>
    {
        #region Methods

        /// <summary>
        /// Reads an object from the current stream.
        /// </summary>
        /// <param name="input">
        /// The ContentReader used to read the object.
        /// </param>
        /// <param name="existingInstance">
        /// An existing object to read into.
        /// </param>
        /// <returns>
        /// Level instance.
        /// </returns>
        protected override SerializableLevel Read(ContentReader input, SerializableLevel existingInstance)
        {
            var levelSerializer = new LevelSerializer();
            return levelSerializer.Deserialize(input.BaseStream);
        }

        #endregion
    }
}