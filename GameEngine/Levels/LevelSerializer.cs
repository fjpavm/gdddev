// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LevelSerializer.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The level serializer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Levels
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;

    using Gdd.Game.Engine.Scenes;

    using Microsoft.Xna.Framework;

    /// <summary>
    /// The level serializer.
    /// </summary>
    public class LevelSerializer
    {
        #region Public Methods

        /// <summary>
        /// The convert to level.
        /// </summary>
        /// <param name="serializableLevel">
        /// The level entity collection.
        /// </param>
        /// <param name="scene">
        /// The scene.
        /// </param>
        /// <returns>
        /// </returns>
        public Level ConvertToLevel(SerializableLevel serializableLevel, Scene scene)
        {
            var level = new Level(false)
                {
                    Author = serializableLevel.Author, 
                    Script = serializableLevel.Script, 
                    StartPosition = serializableLevel.StartPosition
                };

            foreach (SceneComponent sceneComponent in
                serializableLevel.LevelEntityCollection.Select(
                    levelEntity => this.ConvertToSceneComponent(levelEntity, scene)))
            {
                level.Components.Add(sceneComponent);
            }

            return level;
        }

        /// <summary>
        /// The deserialize.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <param name="scene">
        /// The scene.
        /// </param>
        /// <returns>
        /// </returns>
        public Level Deserialize(Stream stream, Scene scene)
        {
            SerializableLevel levelEntityCollection = this.Deserialize(stream);
            return this.ConvertToLevel(levelEntityCollection, scene);
        }

        /// <summary>
        /// The deserialize.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <returns>
        /// </returns>
        public SerializableLevel Deserialize(Stream stream)
        {
            IEnumerable<Type> entityTypes = from levelEntityTypeBinding in LevelScene.LevelEntityTypeBindings
                                            select levelEntityTypeBinding.LevelEntityType;
            var xmlSerializer = new XmlSerializer(typeof(SerializableLevel), entityTypes.ToArray());
            return (SerializableLevel)xmlSerializer.Deserialize(stream);
        }

        /// <summary>
        /// The serialize.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <param name="level">
        /// The level.
        /// </param>
        public void Serialize(Stream stream, Level level)
        {
            SerializableLevel serializableLevel = this.CreateSerializableLevel(level);
            this.Serialize(stream, serializableLevel);
        }

        /// <summary>
        /// The serialize.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <param name="serializableLevel">
        /// The level entity collection.
        /// </param>
        public void Serialize(Stream stream, SerializableLevel serializableLevel)
        {
            IEnumerable<Type> entityTypes = from levelEntityTypeBinding in LevelScene.LevelEntityTypeBindings
                                            select levelEntityTypeBinding.LevelEntityType;
            var xmlSerializer = new XmlSerializer(typeof(SerializableLevel), entityTypes.ToArray());
            xmlSerializer.Serialize(stream, serializableLevel);
        }

        #endregion

        #region Methods

        /// <summary>
        /// The convert to entity.
        /// </summary>
        /// <param name="sceneComponent">
        /// The scene component.
        /// </param>
        /// <returns>
        /// </returns>
        private LevelEntity ConvertToEntity(SceneComponent sceneComponent)
        {
            Type sceneComponentType = sceneComponent.GetType();
            Type levelEntityType = (from levelEntityTypeBinding in LevelScene.LevelEntityTypeBindings
                                    where levelEntityTypeBinding.SceneComponentType == sceneComponentType
                                    select levelEntityTypeBinding.LevelEntityType).FirstOrDefault();
            if (levelEntityType == null)
            {
                return null;
            }

            var levelEntity = (LevelEntity)Activator.CreateInstance(levelEntityType);
            PropertyInfo[] levelEntityPropertyInfos = levelEntityType.GetProperties();
            foreach (PropertyInfo levelEntityPropertyInfo in levelEntityPropertyInfos)
            {
                PropertyInfo sceneComponentPropertyInfo = sceneComponentType.GetProperty(levelEntityPropertyInfo.Name);
                if (sceneComponentPropertyInfo == null)
                {
                    continue;
                }

                object sceneComponentPropertyValue = sceneComponentPropertyInfo.GetValue(sceneComponent, null);
                levelEntityPropertyInfo.SetValue(levelEntity, sceneComponentPropertyValue, null);
            }

            return levelEntity;
        }

        /// <summary>
        /// The convert to scene component.
        /// </summary>
        /// <param name="levelEntity">
        /// The level entity.
        /// </param>
        /// <param name="scene">
        /// The scene.
        /// </param>
        /// <returns>
        /// </returns>
        private SceneComponent ConvertToSceneComponent(LevelEntity levelEntity, Scene scene)
        {
            Type levelEntityType = levelEntity.GetType();
            Type sceneComponentType = (from levelEntityTypeBinding in LevelScene.LevelEntityTypeBindings
                                       where levelEntityTypeBinding.LevelEntityType == levelEntityType
                                       select levelEntityTypeBinding.SceneComponentType).FirstOrDefault();
            if (sceneComponentType == null)
            {
                return null;
            }

            Game game = scene != null ? scene.Game : null;
            var sceneComponent = (SceneComponent)Activator.CreateInstance(sceneComponentType, game);
            PropertyInfo[] levelEntityPropertyInfos = levelEntityType.GetProperties();
            foreach (PropertyInfo levelEntityPropertyInfo in levelEntityPropertyInfos)
            {
                PropertyInfo sceneComponentPropertyInfo = sceneComponentType.GetProperty(levelEntityPropertyInfo.Name);
                if (sceneComponentPropertyInfo == null)
                {
                    continue;
                }

                object levelEntityPropertyValue = levelEntityPropertyInfo.GetValue(levelEntity, null);
                sceneComponentPropertyInfo.SetValue(sceneComponent, levelEntityPropertyValue, null);
            }

            sceneComponent.SetScene(scene);
            return sceneComponent;
        }

        /// <summary>
        /// The create serializable collection.
        /// </summary>
        /// <param name="level">
        /// The level.
        /// </param>
        /// <returns>
        /// </returns>
        private SerializableLevel CreateSerializableLevel(Level level)
        {
            var serializableLevel = new SerializableLevel
                {
                   Author = level.Author, Script = level.Script, StartPosition = level.StartPosition 
                };
            foreach (LevelEntity levelEntity in
                level.Components.Select(sceneComponent => this.ConvertToEntity(sceneComponent)))
            {
                serializableLevel.LevelEntityCollection.Add(levelEntity);
            }

            return serializableLevel;
        }

        #endregion
    }
}