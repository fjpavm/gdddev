using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gdd.Game.Engine.Physics
{
    interface ICollides
    {
        bool OnCollision(Scenes.DrawableSceneComponent dsc);
    }
}
