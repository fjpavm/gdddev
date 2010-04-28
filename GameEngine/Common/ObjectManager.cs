using System;
using System.Collections.Generic;
using Gdd.Game.Engine.Scenes;

namespace Gdd.Game.Engine.Common
{
    // this class is used as the object manager
    public class ObjectManager
    { 
        // I know this looks bad, I'll change it if we have time
        public static Dictionary<int, List<DrawableSceneComponent>> drawableSceneComponents { get; private set; }
        public static Dictionary<int, List<SceneComponent>> sceneComponents { get; private set; }

        public static void AddDrawableSceneComponent(int sceneId, ref DrawableSceneComponent dsc)
        {
            //SetUpLists(sceneId);

            drawableSceneComponents[sceneId].Add(dsc);
        }

        public static void AddSceneComponent(int sceneId, ref SceneComponent sc)
        {
            //SetUpLists(sceneId);

            sceneComponents[sceneId].Add(sc);
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

    }
}
