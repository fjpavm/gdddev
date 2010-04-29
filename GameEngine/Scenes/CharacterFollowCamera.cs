using Microsoft.Xna.Framework;

namespace Gdd.Game.Engine.Scenes
{
    using Levels.Characters;

    public class CharacterFollowCamera : Camera
    {
        private Levels.AnimatedModel.DIRECTION lastDirection;
        private float multiplier, targetMultiplier;
        private Vector3 positionOffset, lookatOffset;

        public CharacterFollowCamera(Microsoft.Xna.Framework.Game game, Vector3 position) : base(game, position) { 
            lastDirection = Levels.AnimatedModel.DIRECTION.RIGHT; 
            multiplier = 1.0f; 
            targetMultiplier = 1.0f;
            lookatOffset = new Vector3(15.0f * multiplier, 10.0f, 0.0f);
            positionOffset = new Vector3(15.0f * multiplier, 15.0f, 35.0f);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            this.position = Hero.GetHeroPosition() + positionOffset;
            this.lookVector = Hero.GetHeroPosition() + lookatOffset;

            this.View = Matrix.CreateLookAt(position, lookVector, Vector3.Up);

            if(lastDirection != Hero.GetHeroDirection()){
                targetMultiplier *= -1.0f;
                lastDirection = Hero.GetHeroDirection();
            }

            if(targetMultiplier != multiplier){
                multiplier += 0.025f * (targetMultiplier);

                if (multiplier > 1.0f)
                    multiplier = 1.0f;
                else if (multiplier < -1.0f)
                    multiplier = -1.0f;

                lookatOffset.X = 15.0f * multiplier;
                positionOffset.X = 15.0f * multiplier;
            }         
        }
    }
}
