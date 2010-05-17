using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gdd.Game.Engine.AI.StateMachines
{
    using Gdd.Game.Engine.Levels;

    class BunnyAnimationStateMachine : StateMachine
    {
        AnimationCommandTest right, left, die;

        public BunnyAnimationStateMachine(object _thisObject) 
        {
            thisObject = _thisObject;
            AnimationState skatingRight = new FlipDirectionAnimationState(_thisObject, "Skating", ModelDirection.Right);
            skatingRight.Repeat = true;
            right = new AnimationCommandTest(AnimationCommandDictionary.register("MovingRightBunny"));

            AnimationState skatingLeft = new FlipDirectionAnimationState(_thisObject, "Skating", ModelDirection.Left);
            skatingLeft.Repeat = true;
            left = new AnimationCommandTest(AnimationCommandDictionary.register("MovingLeftBunny"));

            AnimationState dieing = new AnimationState(_thisObject, "Death");
            dieing.Repeat = false;
            die = new AnimationCommandTest(AnimationCommandDictionary.register("DyingBunny"));

            Transition toDeath = new Transition();
            toDeath.nextState = dieing;
            toDeath.transitionTest = die.thisCommand;

            Transition toRight = new Transition();
            toRight.nextState = skatingRight;
            toRight.transitionTest = right.thisCommand;

            Transition toLeft = new Transition();
            toLeft.nextState = skatingLeft;
            toLeft.transitionTest = left.thisCommand;

            dieing.addTransition(MessageTypes.animationCommand, toLeft);
            dieing.addTransition(MessageTypes.animationCommand, toRight);

            skatingRight.addTransition(MessageTypes.animationCommand, toLeft);
            skatingRight.addTransition(MessageTypes.animationCommand, toDeath);

            skatingLeft.addTransition(MessageTypes.animationCommand, toRight);
            skatingLeft.addTransition(MessageTypes.animationCommand, toDeath);

            InitialState = skatingLeft;
        }

    }
}
