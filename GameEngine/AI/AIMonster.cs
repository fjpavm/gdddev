using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gdd.Game.Engine.Scenes;
using Gdd.Game.Engine.Levels;

namespace Gdd.Game.Engine.AI
{
    using Microsoft.Xna.Framework;
    public class AIMonster : AnimatedModel, IAIEntity, IDebugable
    {
        StateMachine stateMachine;
        private bool debug;
        public bool Debug 
        {
            get { return debug; }
            set { debug = value; }
        }

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
            debug = false;  
        }

        protected AIMonster() : this(null) 
        { 
        }

        public void UpdateAI(GameTime t) 
        {
            stateMachine.Update(t.ElapsedGameTime.TotalSeconds);
            animationStateMachine.Update(t.ElapsedGameTime.TotalSeconds);
            
        }

        public bool ProcessMessage(Message m) 
        {
            bool ret = stateMachine.ProcessMessage(m);
            ret |= animationStateMachine.ProcessMessage(m);
            return ret;
        }

        public Animation.ModelAnimationPlayer Player
        {
            get
            {
                return AnimationPlayer;
            }
        }
    }
}
