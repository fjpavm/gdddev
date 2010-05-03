using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Gdd.Game.Engine.AI.StateMachines
{
    class ChargingBunnyState : State 
    {
        Microsoft.Xna.Framework.Vector2 chargingDirection = new Microsoft.Xna.Framework.Vector2();
        bool directionChanged = false;
        int moveRightAnimationCommand, moveLeftAnimationCommand;

        public ChargingBunnyState() : base("ChargingBunnyState")
        {
            moveRightAnimationCommand = AnimationCommandDictionary.lookUp("MovingRightBunny");
            moveLeftAnimationCommand = AnimationCommandDictionary.lookUp("MovingLeftBunny");
        }

        public override void Update() 
        { 
            //set proper animation
            if (directionChanged)
            {
                if (chargingDirection.X > 0)
                {
                    AIManager.messageQueue.sendMessage(new AnimationCommandMessage(thisObject as IMessageProcessor, thisObject as IMessageProcessor, moveRightAnimationCommand));
                }
                if (chargingDirection.X < 0)
                {
                    AIManager.messageQueue.sendMessage(new AnimationCommandMessage(thisObject as IMessageProcessor, thisObject as IMessageProcessor, moveLeftAnimationCommand));
                }
            }
            //TODO: do physics action

        }

        public override bool ProcessMessage(Message m)
        {
            if(m.MessageType == MessageTypes.characterPosition)
            {
                directionChanged = false;
                Scenes.SceneComponent otherObj = m.from as Scenes.SceneComponent;
                Scenes.SceneComponent thisObj = thisObject as Scenes.SceneComponent;

                //TODO: a reasonable direction changing test
                chargingDirection = otherObj.Position2D - thisObj.Position2D;
                directionChanged = true;
                return true;
            }
            return false;
        }    
    }

    class MoveToSetpointBunnyState : State
    {
        int moveRightAnimationCommand, moveLeftAnimationCommand;
        double lastDistanceSquared;
        int nrOffSetbacks;
        bool movingToNext = false;
        const int left = -1;
        const int right = +1;
        const int maxSetBacks = 500;
        int direction; // -1 left, +1 right
        Vector2 setPoint;

        public MoveToSetpointBunnyState(Vector2 _setPoint) : base("MoveToSetpointBunnyState")
        {
            moveRightAnimationCommand = AnimationCommandDictionary.lookUp("MovingRightBunny");
            moveLeftAnimationCommand = AnimationCommandDictionary.lookUp("MovingLeftBunny");
            setPoint = _setPoint;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            nrOffSetbacks = 0;
            movingToNext = false;
            Scenes.SceneComponent bunny = thisObject as Scenes.SceneComponent;
            lastDistanceSquared = (setPoint - bunny.Position2D).LengthSquared();
            direction = (setPoint.X - bunny.Position2D.X) > 0 ? right : left;
            if (direction == right)
            {
                AIManager.messageQueue.sendMessage(new AnimationCommandMessage(thisObject as IMessageProcessor, thisObject as IMessageProcessor, moveRightAnimationCommand));
            }
            if (direction == left)
            {
                AIManager.messageQueue.sendMessage(new AnimationCommandMessage(thisObject as IMessageProcessor, thisObject as IMessageProcessor, moveLeftAnimationCommand));
            }
        }


        public override void Update()
        {
            base.Update();
            Scenes.SceneComponent bunny = thisObject as Scenes.SceneComponent;
            double distanceSquared = (setPoint - bunny.Position2D).LengthSquared();
            int currentDirection = (setPoint.X - bunny.Position2D.X) > 0 ? right : left;
            if (!movingToNext)
            {
                if (distanceSquared <= 0.1 || currentDirection != direction)
                {
                    //go to next setpoint
                    AIManager.messageQueue.sendMessage(new NextSetPointMessage(thisObject as IMessageProcessor, thisObject as IMessageProcessor));
                    movingToNext = true;
                }
                if (distanceSquared >= lastDistanceSquared)
                {
                    nrOffSetbacks++;
                    //Check nrOffSetbacks against a limit and go to next setpoint if exceded
                    if (!movingToNext && nrOffSetbacks > maxSetBacks)
                    {
                        AIManager.messageQueue.sendMessage(new NextSetPointMessage(thisObject as IMessageProcessor, thisObject as IMessageProcessor));
                        movingToNext = true;
                    }
                    return;
                }
            }
            lastDistanceSquared = distanceSquared;
            nrOffSetbacks = 0;

            //set proper animation
            if (direction != currentDirection) 
            {
                if (direction == right) 
                {
                    AIManager.messageQueue.sendMessage(new AnimationCommandMessage(thisObject as IMessageProcessor, thisObject as IMessageProcessor, moveRightAnimationCommand));
                }
                if (direction == left)
                {
                    AIManager.messageQueue.sendMessage(new AnimationCommandMessage(thisObject as IMessageProcessor, thisObject as IMessageProcessor, moveLeftAnimationCommand));
                }
            }
            //TODO: do physics action
        }
    
    }

    class PatrolBunnyState : StateMachine 
    { 
        public PatrolBunnyState(object thisObject, Vector2 setpoint1, Vector2 setpoint2)
            : base("PatrolBunnyState_StateMachine")
        {
            IState moveToSetpoint1 = new MoveToSetpointBunnyState(setpoint1);
            IState moveToSetpoint2 = new MoveToSetpointBunnyState(setpoint2);

            moveToSetpoint1.setThisObject(thisObject);
            moveToSetpoint2.setThisObject(thisObject);

            Transition t = new Transition();
            t.nextState = moveToSetpoint2;
            t.transitionTest = AlwaysTrueTest.alwaysTrue;
            moveToSetpoint1.addTransition(MessageTypes.toNextSetpoint, t);

            t = new Transition();
            t.nextState = moveToSetpoint1;
            t.transitionTest = AlwaysTrueTest.alwaysTrue;
            moveToSetpoint2.addTransition(MessageTypes.toNextSetpoint, t);

            InitialState = moveToSetpoint1;
            
        }
    }

    class AliveBunnyState : StateMachine 
    {
        DistanceTest patrolToCharge, chargeToPatrol;
        const float patrolTochargeDist = 5.0f;
        const float patrolToChargeHisteresis = 0.2f;
        

        public AliveBunnyState(object thisObject, Vector2 setpoint1, Vector2 setpoint2)
            : base("AliveBunnyState_StateMachine")
        {
            IState patrol = new PatrolBunnyState(thisObject, setpoint1, setpoint2);
            IState charge = new ChargingBunnyState();

            patrolToCharge = new DistanceTest(patrolTochargeDist - patrolToChargeHisteresis / 2);
            chargeToPatrol = new DistanceTest(patrolTochargeDist + patrolToChargeHisteresis / 2);
                        
            charge.setThisObject(thisObject);

            Transition t = new Transition();
            t.nextState = charge;
            t.transitionTest = patrolToCharge.lesser;
            patrol.addTransition(MessageTypes.characterPosition, t);

            t = new Transition();
            t.nextState = patrol;
            t.transitionTest = chargeToPatrol.greater;
            charge.addTransition(MessageTypes.characterPosition, t);
           
            InitialState = patrol;   
        }
    }


    class DeadBunnyState : State
    {
        int animationCommand;

        public DeadBunnyState()
            : base("DeadBunnyState")
        {
            animationCommand = AnimationCommandDictionary.lookUp("DyingBunny");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Message m = new AnimationCommandMessage(thisObject as IMessageProcessor, thisObject as IMessageProcessor, animationCommand);
            AIManager.messageQueue.sendMessage(m);
        }

    }

    public class BunnyStateMachine : StateMachine
    {
        public BunnyStateMachine(object thisObject, Vector2 setpoint1, Vector2 setpoint2)
            : base("BunnyStateMachine")
        {
            IState alive = new AliveBunnyState(thisObject, setpoint1, setpoint2);
            IState dead = new DeadBunnyState();

                                   
            dead.setThisObject(thisObject);

            Transition t = new Transition();
            t.nextState = dead;
            t.transitionTest = AlwaysTrueTest.alwaysTrue;
            alive.addTransition(MessageTypes.die, t);

            t = new Transition();
            t.nextState = alive;
            t.transitionTest = AlwaysTrueTest.alwaysTrue;
            dead.addTransition(MessageTypes.resurect, t);
           
            InitialState = alive;   
        }

    }
}
