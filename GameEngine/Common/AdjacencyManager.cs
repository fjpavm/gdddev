using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Gdd.Game.Engine.Scenes;
using FarseerGames.FarseerPhysics.Collisions;
using Gdd.Game.Engine.Scenes.Lights;

namespace Gdd.Game.Engine.Common
{
    // work in progress, planning on using this manager as a checker for adjacency
    public class AdjacencyManager
    {
        public static void PopulateAdjacency(int sceneId)
        {
            AABB aabb1, aabb2;
            foreach (DrawableSceneComponent adjSceneDSC in ObjectManager.drawableSceneComponents[sceneId])
            {
                adjSceneDSC.ClearAdjacencyLists();

                /*foreach (DrawableSceneComponent sceneDSC in ObjectManager.drawableSceneComponents[sceneId])
                {
                    aabb1 = adjSceneDSC.aabb;
                    aabb2 = sceneDSC.aabb;
                    if (adjSceneDSC != sceneDSC && adjSceneDSC.aabb != null && AABB.Intersect(ref aabb1, ref aabb2))
                    {
                        adjSceneDSC.AddAdjacentDrawableSceneComponent(sceneDSC);
                    }
                }*/

                foreach (SceneComponent sceneDS in ObjectManager.sceneComponents[sceneId])
                {
                    aabb1 = adjSceneDSC.aabb;
                    aabb2 = sceneDS.aabb;
                    if (adjSceneDSC.aabb != null && sceneDS.aabb != null && AABB.Intersect(ref aabb1, ref aabb2))
                    {
                        if (sceneDS is DrawableSceneComponent && !(sceneDS is PointLight))
                        {
                            adjSceneDSC.AddAdjacentDrawableSceneComponent((DrawableSceneComponent)sceneDS);
                        }
                        else
                        {
                            adjSceneDSC.AddAdjacentSceneComponent(sceneDS);
                        }
                    }
                }
            }
        }
    }
}
