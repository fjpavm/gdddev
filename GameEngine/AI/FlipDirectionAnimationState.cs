using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gdd.Game.Engine.AI
{
    class FlipDirectionAnimationState : AnimationState
    {
        Levels.StaticModel.DIRECTION wantedDirection;

        public FlipDirectionAnimationState(object _thisObject, string _animationName, Levels.StaticModel.DIRECTION _wantedDirection) : base(_thisObject, _animationName)
        {
            wantedDirection = _wantedDirection;
            this.name = _animationName + wantedDirection + "_FlipAnimationState";
        }

        public override void Update()
        {
            base.Update();
            AIMonster thisMonster = thisObject as AIMonster;
            if (!first && thisMonster.Direction != wantedDirection)
            {
                thisMonster.Flip();
            }
        }
    }
}
