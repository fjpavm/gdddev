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
        public GeomDC(Scenes.DrawableSceneComponent dsc, Geom g) : base(g.Body, g) { thisObject = dsc; this.OnCollision += OnCollisionFunction; }

        public bool OnCollisionFunction(Geom geom1, Geom geom2, ContactList contactList)
        {
            GeomDC g1 = geom1 as GeomDC;
            GeomDC g2 = geom2 as GeomDC;
            if(g1 == null || g2 == null){
                return true;
            }
            ICollides col1 = g1.thisObject as ICollides;
            if(col1 == null){
                return true;
            }
            return col1.OnCollision(g2.thisObject);
        }
    }
}
