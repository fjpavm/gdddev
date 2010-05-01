using System;
using System.Collections.Generic;
using Gdd.Game.Engine.Scenes;

namespace Gdd.Game.Engine.Common
{
    // this class is used as the object manager
    public static class ObjectManager
    { 
        private class DrawableOrderComparer: IComparer<DrawableSceneComponent>
        {
            private static DrawableOrderComparer defaultComparer;

            static DrawableOrderComparer ()
            {
                defaultComparer = new DrawableOrderComparer();
            }

            public static DrawableOrderComparer Default { get {
                return defaultComparer; } }
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
        }
        private class UpdateableOrderComparer : IComparer<SceneComponent>
        {
            private static UpdateableOrderComparer defaultComparer;

            static UpdateableOrderComparer()
            {
                defaultComparer = new UpdateableOrderComparer();
            }

            public static UpdateableOrderComparer Default
            {
                get
                {
                    return defaultComparer;
                }
            }
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
        }

        // I know this looks bad, I'll change it if we have time
        public static Dictionary<int, List<DrawableSceneComponent>> drawableSceneComponents { get; private set; }
        public static Dictionary<int, List<SceneComponent>> sceneComponents { get; private set; }

        public static void AddDrawableSceneComponent(int sceneId, ref DrawableSceneComponent dsc)
        {
            //SetUpLists(sceneId);
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

        public static void AddSceneComponent(int sceneId, ref SceneComponent sc)
        {
            //SetUpLists(sceneId);
            List<SceneComponent> updateableComponents;
            if (sceneComponents.TryGetValue(sceneId, out updateableComponents))
            {
                int index = updateableComponents.BinarySearch(sc, UpdateableOrderComparer.Default);
                if (index < 0)
                {
                    index = ~index;
                    while ((index < updateableComponents.Count) && (updateableComponents[index].UpdateOrder == sc.UpdateOrder))
                    {
                        index++;
                    }

                    updateableComponents.Insert(index, sc);
                }
            }
        }

        public static void SetUpLists(int sceneId)
        {
            if (drawableSceneComponents == null)
                drawableSceneComponents = new Dictionary<int, List<DrawableSceneComponent>>();

            if (sceneComponents == null)
                sceneComponents = new Dictionary<int, List<SceneComponent>>();

            if (!sceneComponents.ContainsKey(sceneId))
                sceneComponents.Add(sceneId, new List<SceneComponent>());

            if (!drawableSceneComponents.ContainsKey(sceneId))
                drawableSceneComponents.Add(sceneId, new List<DrawableSceneComponent>());
        }

        public static void Dispose(int sceneId)
        {
            // dispose the drawable scene components
            foreach (DrawableSceneComponent dsc in drawableSceneComponents[sceneId])
                dsc.Dispose();

            // dispose the scene components
            foreach (SceneComponent sc in sceneComponents[sceneId])
                sc.Dispose();

            // remove the scene from the lists
            drawableSceneComponents.Remove(sceneId);
            sceneComponents.Remove(sceneId);
        }

        public static void ClearSceneComponents(int sceneId)
        {
            sceneComponents[sceneId].Clear();
            drawableSceneComponents[sceneId].Clear();
            GC.Collect();
        }

        public static void RemoveSceneComponent(int sceneId, ref SceneComponent sc)
        {
            sceneComponents[sceneId].Remove(sc);
        }

        public static void RemoveDrawableSceneComponent(int sceneId, ref DrawableSceneComponent dsc)
        {
            drawableSceneComponents[sceneId].Remove(dsc);
        }

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
                    while ((index < updateableComponents.Count) && (updateableComponents[index].UpdateOrder == sc.UpdateOrder))
                    {
                        index++;
                    }

                    updateableComponents.Insert(index, sc);
                }
            }            
        }
    }
}
