using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Gdd.Game.Engine.AI.StateMachines
{
    class DistanceTest
    {
        float squaredDistanceThreshold;

        public DistanceTest(float dist)
        {
            squaredDistanceThreshold = dist*dist;
        }

        float calSquaredDistance(Message msg, object obj)
        {
            Scenes.SceneComponent otherObj = msg.from as Scenes.SceneComponent;
            Scenes.SceneComponent thisObj = obj as Scenes.SceneComponent;
            return (otherObj.Position2D - thisObj.Position2D).LengthSquared();
        }

        public bool greater(Message msg, object obj)
        {
             return calSquaredDistance(msg, obj) > squaredDistanceThreshold;
        }

        public bool lesser(Message msg, object obj)
        {
             return calSquaredDistance(msg, obj) < squaredDistanceThreshold;
        }

    }
}
