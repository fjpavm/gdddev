﻿// --------------------------------------------------------------------------------------------------------------------
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

    /// <summary>
    /// The level serializer.
    /// </summary>
    public class LevelSerializer
    {
        #region Public Methods

        /// <summary>
        /// The convert to level.
        /// </summary>
        /// <param name="levelEntityCollection">
        /// The level entity collection.
        /// </param>
        /// <param name="scene">
        /// The scene.
        /// </param>
        /// <returns>
        /// </returns>
        public Level ConvertToLevel(LevelEntityCollection levelEntityCollection, Scene scene)
        {
            var level = new Level(false)
                {
                   Author = levelEntityCollection.Author, Script = levelEntityCollection.Script 
                };

            foreach (SceneComponent sceneComponent in
                levelEntityCollection.Select(levelEntity => this.ConvertToSceneComponent(levelEntity, scene)))
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
            LevelEntityCollection levelEntityCollection = this.Deserialize(stream);
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
        public LevelEntityCollection Deserialize(Stream stream)
        {
            IEnumerable<Type> entityTypes = from levelEntityTypeBinding in LevelScene.LevelEntityTypeBindings
                                            select levelEntityTypeBinding.LevelEntityType;
            var xmlSerializer = new XmlSerializer(typeof(LevelEntityCollection), entityTypes.ToArray());
            return (LevelEntityCollection)xmlSerializer.Deserialize(stream);
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
            LevelEntityCollection levelEntityCollection = this.CreateSerializableCollection(level);
            this.Serialize(stream, levelEntityCollection);
        }

        /// <summary>
        /// The serialize.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <param name="levelEntityCollection">
        /// The level entity collection.
        /// </param>
        public void Serialize(Stream stream, LevelEntityCollection levelEntityCollection)
        {
            IEnumerable<Type> entityTypes = from levelEntityTypeBinding in LevelScene.LevelEntityTypeBindings
                                            select levelEntityTypeBinding.LevelEntityType;
            var xmlSerializer = new XmlSerializer(typeof(LevelEntityCollection), entityTypes.ToArray());
            xmlSerializer.Serialize(stream, levelEntityCollection);
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

            var sceneComponent = (SceneComponent)Activator.CreateInstance(sceneComponentType, scene.Game);
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
        private LevelEntityCollection CreateSerializableCollection(Level level)
        {
            var levelEntityCollection = new LevelEntityCollection { Author = level.Author, Script = level.Script };
            foreach (LevelEntity levelEntity in
                level.Components.Select(sceneComponent => this.ConvertToEntity(sceneComponent)))
            {
                levelEntityCollection.Add(levelEntity);
            }

            return levelEntityCollection;
        }

        #endregion
    }
}