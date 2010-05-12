using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gdd.Game.Engine.AI
{
    class AnimationState : State
    {
        string animationName;
        Animation.AnimationClip animationClip;
        bool repeat = true;
        protected bool first;
        public bool Repeat 
        { 
            set
            {
                repeat = value;
            }
            get 
            {
                return repeat;
            }
        }

        public string AnimationName {
            set {
                animationName = value;
                AIMonster thisMonster = thisObject as AIMonster;
                animationClip = thisMonster.skinningData.AnimationClips[animationName];
            }
        }

        public AnimationState() : base("AnimationState")
        { 
        
        }

        public AnimationState(object _thisObject, string _animationName) : base(_animationName + "_AnimationState") {
            setThisObject(_thisObject);
            AnimationName = _animationName;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            AIMonster thisMonster = thisObject as AIMonster;
            bool playingPrevious = !thisMonster.Player.IsStopped;
            if (playingPrevious) thisMonster.Player.RunUntilEnd();
            first = true;
        }

        public override void Update(double timeDiff)
        {
            base.Update(timeDiff);
            AIMonster thisMonster = thisObject as AIMonster;
            if ( first && thisMonster.Player.IsStopped) 
            {
                thisMonster.Player.SetClip(animationClip);
                first = false;
                if (repeat)
                {
                    thisMonster.Player.StartClip();
                }
                else 
                {
                    thisMonster.Player.RunOnce();
                }
                return;
            }
            
        }


    }
}
