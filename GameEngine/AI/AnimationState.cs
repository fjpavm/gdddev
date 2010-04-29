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
        bool first;
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
            bool playingPrevious = !thisMonster.getPlayer().IsStopped;
            if (playingPrevious) thisMonster.getPlayer().RunUntilEnd();
            first = true;
        }

        public override void Update()
        {
            base.Update();
            AIMonster thisMonster = thisObject as AIMonster;
            if ( first && thisMonster.getPlayer().IsStopped) 
            {
                thisMonster.getPlayer().SetClip(animationClip);
                first = false;
                if (repeat)
                {
                    thisMonster.getPlayer().StartClip();
                }
                else 
                {
                    thisMonster.getPlayer().RunOnce();
                }
                return;
            }
            
        }


    }
}
