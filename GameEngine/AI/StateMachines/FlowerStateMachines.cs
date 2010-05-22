using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gdd.Game.Engine.AI.StateMachines
{


    class IdleFlowerState : State 
    {
        int animationCommand;

        public IdleFlowerState() : base("IdleFlowerState")
        {
            animationCommand = AnimationCommandDictionary.lookUp("IdleFlower");
        }

        public override void OnEnter() 
        {
            base.OnEnter(); 
            Message m = new AnimationCommandMessage(thisObject as IMessageProcessor, thisObject as IMessageProcessor, animationCommand);
            AIManager.messageQueue.sendMessage(m);
        }

        //public override void Update() { Console.WriteLine(name + ".Update()"); }

        //public override void OnExit() { Console.WriteLine(name + ".OnExit()"); }

        //public override bool ProcessMessage(Message m) { Console.WriteLine(name + ".ProcessMessage({0})", m.MessageType); return true; }

    
    }

    class EvilFlowerState : State
    {
        int animationCommand;

        public EvilFlowerState()
            : base("EvilFlowerState")
        {
            animationCommand = AnimationCommandDictionary.lookUp("EvilFlower");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Message m = new AnimationCommandMessage(thisObject as IMessageProcessor, thisObject as IMessageProcessor, animationCommand);
            AIManager.messageQueue.sendMessage(m);
        }

        //public override void Update() { Console.WriteLine(name + ".Update()"); }

        //public override void OnExit() { Console.WriteLine(name + ".OnExit()"); }

        //public override bool ProcessMessage(Message m) { Console.WriteLine(name + ".ProcessMessage({0})", m.MessageType); return true; }

    }

    class AttackingFlowerState : State
    {
        int animationCommand;

        public AttackingFlowerState()
            : base("AttackingFlowerState")
        {
            animationCommand = AnimationCommandDictionary.lookUp("AttackingFlower");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Message m = new AnimationCommandMessage(thisObject as IMessageProcessor, thisObject as IMessageProcessor, animationCommand);
            AIManager.messageQueue.sendMessage(m);
            Audio.PlayAttackingFlower();
        }

        //public override void Update() { Console.WriteLine(name + ".Update()"); }

        //public override void OnExit() { Console.WriteLine(name + ".OnExit()"); }

        //public override bool ProcessMessage(Message m) { Console.WriteLine(name + ".ProcessMessage({0})", m.MessageType); return true; }

    }

    public class AliveFlowerState : StateMachine
    {
        DistanceTest idleToEvil, evilToIdle, evilToAttack, attackToEvil;
        const float idleToEvilDist = 20.0f;
        const float idleToEvilHisteresis = 0.2f;
        const float evilToAttackDist = 5.0f;
        const float evilToAttackHisteresis = 0.2f;

        public AliveFlowerState(object thisObject)
            : base("AliveFlowerState_StateMachine")
        {
            this.thisObject = thisObject;
            IState idle = new IdleFlowerState();
            IState evil = new EvilFlowerState();
            IState attack = new AttackingFlowerState();
            
            idleToEvil = new DistanceTest(idleToEvilDist - idleToEvilHisteresis/2);            
            evilToIdle = new DistanceTest(idleToEvilDist + idleToEvilHisteresis/2);
            evilToAttack = new DistanceTest(evilToAttackDist - evilToAttackHisteresis/2);
            attackToEvil = new DistanceTest(evilToAttackDist + evilToAttackHisteresis/2);
            
            idle.setThisObject(thisObject);
            evil.setThisObject(thisObject);
            attack.setThisObject(thisObject);

            Transition t = new Transition();
            t.nextState = evil;
            t.transitionTest = idleToEvil.lesser;
            idle.addTransition(MessageTypes.characterPosition, t);

            t = new Transition();
            t.nextState = idle;
            t.transitionTest = evilToIdle.greater;
            evil.addTransition(MessageTypes.characterPosition, t);
            t = new Transition();
            t.nextState = attack;
            t.transitionTest = evilToAttack.lesser;
            evil.addTransition(MessageTypes.characterPosition, t);

            t = new Transition();
            t.nextState = evil;
            t.transitionTest = attackToEvil.greater;
            attack.addTransition(MessageTypes.characterPosition, t);

            InitialState = idle;
            
        }

    }




    class DeadFlowerState : State
    {
        int animationCommand;

        public DeadFlowerState()
            : base("DeadFlowerState")
        {
            animationCommand = AnimationCommandDictionary.lookUp("DyingFlower");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Message m = new AnimationCommandMessage(thisObject as IMessageProcessor, thisObject as IMessageProcessor, animationCommand);
            AIManager.messageQueue.sendMessage(m);
            Audio.PlayDyingFlower();
        }

        //public override void Update() { Console.WriteLine(name + ".Update()"); }

        //public override void OnExit() { Console.WriteLine(name + ".OnExit()"); }

        //public override bool ProcessMessage(Message m) { Console.WriteLine(name + ".ProcessMessage({0})", m.MessageType); return true; }

    }




    public class FlowerStateMachine : StateMachine
    {
        static uint nrUnnamedFlowerStateMachines = 0;

        //NOTE: thisObject should be our game object
        public FlowerStateMachine(object thisObject)
            : this(thisObject,"UnnamedFlowerStateMachine" + ++nrUnnamedFlowerStateMachines)
		{
		}

        //NOTE: thisObject should be our game object
        public FlowerStateMachine(object thisObject, string name)
            : base(name)
        {
            setThisObject(thisObject);

            IState alive = new AliveFlowerState(thisObject);
            IState dead = new DeadFlowerState();

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
