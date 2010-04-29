using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gdd.Game.Engine.AI.StateMachines
{
    class FlowerAnimationStateMachine : StateMachine
    {
        AnimationCommandTest idle, evil, attack, die;

        public FlowerAnimationStateMachine(object _thisObject) 
        {
            thisObject = _thisObject;
            AnimationState idleAnim = new AnimationState(_thisObject, "idle");
            idleAnim.Repeat = true;
            idle = new AnimationCommandTest(AnimationCommandDictionary.register("IdleFlower"));

            AnimationState attackAnim = new AnimationState(_thisObject, "attack");
            attackAnim.Repeat = true;
            attack = new AnimationCommandTest(AnimationCommandDictionary.register("AttackingFlower"));

            AnimationState evilAnim = new AnimationState(_thisObject, "evil");
            evilAnim.Repeat = true;
            evil = new AnimationCommandTest(AnimationCommandDictionary.register("EvilFlower"));

            AnimationState dieAnim = new AnimationState(_thisObject, "die");
            dieAnim.Repeat = false;
            die = new AnimationCommandTest(AnimationCommandDictionary.register("DyingFlower"));

            Transition toDeath = new Transition();
            toDeath.nextState = dieAnim;
            toDeath.transitionTest = die.thisCommand;

            Transition toIdle = new Transition();
            toIdle.nextState = idleAnim;
            toIdle.transitionTest = idle.thisCommand;

            Transition toAttack = new Transition();
            toAttack.nextState = attackAnim;
            toAttack.transitionTest = attack.thisCommand;

            Transition toEvil = new Transition();
            toEvil.nextState = evilAnim;
            toEvil.transitionTest = evil.thisCommand;

            idleAnim.addTransition(MessageTypes.animationCommand, toEvil );
            idleAnim.addTransition(MessageTypes.animationCommand, toAttack );
            idleAnim.addTransition(MessageTypes.animationCommand, toDeath );

            evilAnim.addTransition(MessageTypes.animationCommand, toIdle);
            evilAnim.addTransition(MessageTypes.animationCommand, toAttack);
            evilAnim.addTransition(MessageTypes.animationCommand, toDeath);

            attackAnim.addTransition(MessageTypes.animationCommand, toIdle);
            attackAnim.addTransition(MessageTypes.animationCommand, toEvil);
            attackAnim.addTransition(MessageTypes.animationCommand, toDeath);

            dieAnim.addTransition(MessageTypes.animationCommand, toIdle);
            dieAnim.addTransition(MessageTypes.animationCommand, toEvil);
            dieAnim.addTransition(MessageTypes.animationCommand, toAttack);

            InitialState = idleAnim;
        }
    }
}
