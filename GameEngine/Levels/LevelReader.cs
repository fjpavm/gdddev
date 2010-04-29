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
    using System.Linq;
    using System.Xml.Serialization;

    using Gdd.Game.Engine.Scenes;

    using Microsoft.Xna.Framework.Content;

    /// <summary>
    /// The level reader.
    /// </summary>
    internal class LevelReader : ContentTypeReader<Level>
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
        protected override Level Read(ContentReader input, Level existingInstance)
        {
            var xmlSerializer = new XmlSerializer(typeof(Level), typeof(DrawableSceneComponent).GetSubTypes().ToArray());
            return xmlSerializer.Deserialize(input.BaseStream) as Level;
        }

        #endregion
    }
}