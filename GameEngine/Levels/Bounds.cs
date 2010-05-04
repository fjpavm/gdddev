using System;
using Gdd.Game.Engine.Scenes;
using Microsoft.Xna.Framework;
using FarseerGames.FarseerPhysics.Dynamics;
using FarseerGames.FarseerPhysics.Collisions;
using FarseerGames.FarseerPhysics.Factories;
using Gdd.Game.Engine.Levels.Characters;

namespace Gdd.Game.Engine.Levels
{
    public class Bounds : DrawableSceneComponent
    {
        public Vector2 Size {get; set;}
        public bool isColliding {get; private set;}

        public Bounds(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            Size = Vector2.Zero;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            this.PhysicsBody = BodyFactory.Instance.CreateRectangleBody(Size.X, Size.Y, 1.0f);
            this.PhysicsGeometry = GeomFactory.Instance.CreateRectangleGeom(PhysicsBody, Size.X, Size.Y);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.PhysicsBody.Position = pos2D;

            isColliding = this.PhysicsGeometry.Collide(Hero.HeroGeometry);

            if(isColliding){
                bool ble = false; 
            }
            else{
                bool ble = false; 
            }
        }

    }
}
