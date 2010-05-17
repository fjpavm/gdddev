// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectManager.cs" company="UAD">
//   Game Design and Development
// </copyright>
// <summary>
//   The object manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Gdd.Game.Engine.Common
{
    using System;
    using System.Collections.Generic;

    using Gdd.Game.Engine.Scenes;

    // this class is used as the object manager
    /// <summary>
    /// The object manager.
    /// </summary>
    public static class ObjectManager
    {
        // I know this looks bad, I'll change it if we have time
        #region Properties

        /// <summary>
        /// Gets drawableSceneComponents.
        /// </summary>
        public static Dictionary<int, List<DrawableSceneComponent>> drawableSceneComponents { get; private set; }

        /// <summary>
        /// Gets sceneComponents.
        /// </summary>
        public static Dictionary<int, List<SceneComponent>> sceneComponents { get; private set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// The add drawable scene component.
        /// </summary>
        /// <param name="sceneId">
        /// The scene id.
        /// </param>
        /// <param name="dsc">
        /// The dsc.
        /// </param>
        public static void AddDrawableSceneComponent(int sceneId, ref DrawableSceneComponent dsc)
        {
            // SetUpLists(sceneId);
            List<DrawableSceneComponent> drawableComponents;
            if (drawableSceneComponents.TryGetValue(sceneId, out drawableComponents))
            {
                int index = drawableComponents.BinarySearch(dsc, DrawableOrderComparer.Default);
                if (index < 0)
                {
                    index = ~index;
                    while ((index < drawableComponents.Count) && (drawableComponents[index].DrawOrder == dsc.DrawOrder))
                    {
                        index++;
                    }

                    drawableComponents.Insert(index, dsc);
                }
            }
        }

        /// <summary>
        /// The add scene component.
        /// </summary>
        /// <param name="sceneId">
        /// The scene id.
        /// </param>
        /// <param name="sc">
        /// The sc.
        /// </param>
        public static void AddSceneComponent(int sceneId, ref SceneComponent sc)
        {
            // SetUpLists(sceneId);
            List<SceneComponent> updateableComponents;
            if (sceneComponents.TryGetValue(sceneId, out updateableComponents))
            {
                int index = updateableComponents.BinarySearch(sc, UpdateableOrderComparer.Default);
                if (index < 0)
                {
                    index = ~index;
                    while ((index < updateableComponents.Count) &&
                           (updateableComponents[index].UpdateOrder == sc.UpdateOrder))
                    {
                        index++;
                    }

                    updateableComponents.Insert(index, sc);
                }
            }
        }

        /// <summary>
        /// The clear scene components.
        /// </summary>
        /// <param name="sceneId">
        /// The scene id.
        /// </param>
        public static void ClearSceneComponents(int sceneId)
        {
            sceneComponents[sceneId].Clear();
            drawableSceneComponents[sceneId].Clear();
            GC.Collect();
        }

        /// <summary>
        /// The dispose.
        /// </summary>
        /// <param name="sceneId">
        /// The scene id.
        /// </param>
        public static void Dispose(int sceneId)
        {
            // dispose the drawable scene components
            foreach (DrawableSceneComponent dsc in drawableSceneComponents[sceneId])
            {
                dsc.Dispose();
            }

            // dispose the scene components
            foreach (SceneComponent sc in sceneComponents[sceneId])
            {
                sc.Dispose();
            }

            // remove the scene from the lists
            drawableSceneComponents.Remove(sceneId);
            sceneComponents.Remove(sceneId);
        }

        /// <summary>
        /// The draw order changed.
        /// </summary>
        /// <param name="sceneId">
        /// The scene id.
        /// </param>
        /// <param name="dsc">
        /// The dsc.
        /// </param>
        public static void DrawOrderChanged(int sceneId, DrawableSceneComponent dsc)
        {
            List<DrawableSceneComponent> drawableComponents;
            if (drawableSceneComponents.TryGetValue(sceneId, out drawableComponents))
            {
                drawableComponents.Remove(dsc);
                int index = drawableComponents.BinarySearch(dsc, DrawableOrderComparer.Default);
                if (index < 0)
                {
                    index = ~index;
                    while ((index < drawableComponents.Count) && (drawableComponents[index].DrawOrder == dsc.DrawOrder))
                    {
                        index++;
                    }

                    drawableComponents.Insert(index, dsc);
                }
            }
        }

        /// <summary>
        /// The remove drawable scene component.
        /// </summary>
        /// <param name="sceneId">
        /// The scene id.
        /// </param>
        /// <param name="dsc">
        /// The dsc.
        /// </param>
        public static void RemoveDrawableSceneComponent(int sceneId, ref DrawableSceneComponent dsc)
        {
            drawableSceneComponents[sceneId].Remove(dsc);
        }

        /// <summary>
        /// The remove scene component.
        /// </summary>
        /// <param name="sceneId">
        /// The scene id.
        /// </param>
        /// <param name="sc">
        /// The sc.
        /// </param>
        public static void RemoveSceneComponent(int sceneId, ref SceneComponent sc)
        {
            sceneComponents[sceneId].Remove(sc);
        }

        /// <summary>
        /// The set up lists.
        /// </summary>
        /// <param name="sceneId">
        /// The scene id.
        /// </param>
        public static void SetUpLists(int sceneId)
        {
            if (drawableSceneComponents == null)
            {
                drawableSceneComponents = new Dictionary<int, List<DrawableSceneComponent>>();
            }

            if (sceneComponents == null)
            {
                sceneComponents = new Dictionary<int, List<SceneComponent>>();
            }

            if (!sceneComponents.ContainsKey(sceneId))
            {
                sceneComponents.Add(sceneId, new List<SceneComponent>());
            }

            if (!drawableSceneComponents.ContainsKey(sceneId))
            {
                drawableSceneComponents.Add(sceneId, new List<DrawableSceneComponent>());
            }
        }

        /// <summary>
        /// The update order changed.
        /// </summary>
        /// <param name="sceneId">
        /// The scene id.
        /// </param>
        /// <param name="sc">
        /// The sc.
        /// </param>
        public static void UpdateOrderChanged(int sceneId, SceneComponent sc)
        {
            List<SceneComponent> updateableComponents;
            if (sceneComponents.TryGetValue(sceneId, out updateableComponents))
            {
                updateableComponents.Remove(sc);
                int index = updateableComponents.BinarySearch(sc, UpdateableOrderComparer.Default);
                if (index < 0)
                {
                    index = ~index;
                    while ((index < updateableComponents.Count) &&
                           (updateableComponents[index].UpdateOrder == sc.UpdateOrder))
                    {
                        index++;
                    }

                    updateableComponents.Insert(index, sc);
                }
            }
        }

        #endregion

        /// <summary>
        /// The drawable order comparer.
        /// </summary>
        private class DrawableOrderComparer : IComparer<DrawableSceneComponent>
        {
            #region Constants and Fields

            /// <summary>
            /// The default comparer.
            /// </summary>
            private static readonly DrawableOrderComparer defaultComparer;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes static members of the <see cref="DrawableOrderComparer"/> class.
            /// </summary>
            static DrawableOrderComparer()
            {
                defaultComparer = new DrawableOrderComparer();
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets Default.
            /// </summary>
            public static DrawableOrderComparer Default
            {
                get
                {
                    return defaultComparer;
                }
            }

            #endregion

            #region Implemented Interfaces

            #region IComparer<DrawableSceneComponent>

            /// <summary>
            /// The compare.
            /// </summary>
            /// <param name="x">
            /// The x.
            /// </param>
            /// <param name="y">
            /// The y.
            /// </param>
            /// <returns>
            /// The compare.
            /// </returns>
            public int Compare(DrawableSceneComponent x, DrawableSceneComponent y)
            {
                if ((x == null) && (y == null))
                {
                    return 0;
                }

                if (x != null)
                {
                    if (y == null)
                    {
                        return -1;
                    }

                    if (x.Equals(y))
                    {
                        return 0;
                    }

                    if (x.DrawOrder < y.DrawOrder)
                    {
                        return -1;
                    }
                }

                return 1;
            }

            #endregion

            #endregion
        }

        /// <summary>
        /// The updateable order comparer.
        /// </summary>
        private class UpdateableOrderComparer : IComparer<SceneComponent>
        {
            #region Constants and Fields

            /// <summary>
            /// The default comparer.
            /// </summary>
            private static readonly UpdateableOrderComparer defaultComparer;

            #endregion

            #region Constructors and Destructors

            /// <summary>
            /// Initializes static members of the <see cref="UpdateableOrderComparer"/> class.
            /// </summary>
            static UpdateableOrderComparer()
            {
                defaultComparer = new UpdateableOrderComparer();
            }

            #endregion

            #region Properties

            /// <summary>
            /// Gets Default.
            /// </summary>
            public static UpdateableOrderComparer Default
            {
                get
                {
                    return defaultComparer;
                }
            }

            #endregion

            #region Implemented Interfaces

            #region IComparer<SceneComponent>

            /// <summary>
            /// The compare.
            /// </summary>
            /// <param name="x">
            /// The x.
            /// </param>
            /// <param name="y">
            /// The y.
            /// </param>
            /// <returns>
            /// The compare.
            /// </returns>
            public int Compare(SceneComponent x, SceneComponent y)
            {
                if ((x == null) && (y == null))
                {
                    return 0;
                }

                if (x != null)
                {
                    if (y == null)
                    {
                        return -1;
                    }

                    if (x.Equals(y))
                    {
                        return 0;
                    }

                    if (x.UpdateOrder < y.UpdateOrder)
                    {
                        return -1;
                    }
                }

                return 1;
            }

            #endregion

            #endregion
        }
    }
}