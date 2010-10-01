using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gdd.Game.Engine.Scenes;
using Gdd.Game.Engine.Levels;
using Gdd.Game.Engine.Physics;

namespace Gdd.Game.Engine.AI
{
    using Microsoft.Xna.Framework;
    public class AIMonster : AnimatedModel, IAIEntity, IDebugable, ICollides
    {
        StateMachine stateMachine;

        const float minLinearMoment = 10.0f;

        public int life;
        public bool hasPaint = true;

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
            hasPaint = true;
        }

        public bool OnCollision(DrawableSceneComponent dsc) 
        {
            Ground g = dsc as Ground;
            if (g != null)
            {
                return true;
            }
            Levels.Characters.Hero h = dsc as Levels.Characters.Hero;
            if (life <= 0)
            {
                if (h != null)
                {
                    if (hasPaint) 
                    {
                        Levels.Characters.Hero.IncreaseLife();
                        hasPaint = false;
                        this.Visible = false;
                    }
                }
                return false;
            }
            AI.AIMonster ai = dsc as AI.AIMonster;
            if (ai != null)
            {
                return true;
            }
            if (h == null)
            {
                if (dsc.PhysicsBody.LinearVelocity.Length()*dsc.PhysicsBody.Mass > minLinearMoment)
                {
                    life--;
                    if (life == 0) 
                    {
                        Message m = new Message
                        {
                            MessageType = MessageTypes.die,
                            timeDelivery = 0,
                            to = this
                        };
                        AIManager.messageQueue.sendMessage( m );
                    }
                    return false;
                }
            }
            return true;
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
