using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerGames.FarseerPhysics.Collisions;

namespace Gdd.Game.Engine.Physics
{
    public class GeomDC : Geom
    {
        public Scenes.DrawableSceneComponent thisObject;
        public GeomDC(Scenes.DrawableSceneComponent dsc,Geom g) : base(g.Body, g) { thisObject = dsc;}
    }
}
