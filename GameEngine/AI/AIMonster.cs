using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gdd.Game.Engine.Scenes;
using Gdd.Game.Engine.Levels;

namespace Gdd.Game.Engine.AI
{
    using Microsoft.Xna.Framework;
    public class AIMonster : AnimatedModel, IAIEntity
    {
        StateMachine stateMachine;

        // to manage animation transitions
        StateMachine animationStateMachine;

        public StateMachine MonsterStateMachine 
        {
            set 
            {
                stateMachine = value;
            }
        }

        public StateMachine AnimationStateMachine
        {
            set
            {
                animationStateMachine = value;
            }

            get 
            {
                return animationStateMachine;
            }
        }

        public AIMonster(Game game) : base(game)
        { 

        }

        protected AIMonster() : this(null) 
        { 
        }

        public void UpdateAI(GameTime t) 
        {
            stateMachine.Update();
            animationStateMachine.Update();
            
        }

        public bool ProcessMessage(Message m) 
        {
            bool ret = stateMachine.ProcessMessage(m);
            ret |= animationStateMachine.ProcessMessage(m);
            return ret;
        }

        //FrankM: Just a hack to get things working without worring about serialing error
        public Animation.ModelAnimationPlayer getPlayer()
        {
            return AnimationPlayer;
        }


    }
}
