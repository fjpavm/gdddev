using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gdd.Game.Engine.AI.StateMachines
{
    class AnimationCommandTest
    {
        int command;

        public AnimationCommandTest(int animationCommand)
        {
            command = animationCommand;
        }

        /*  Assumes message is AnimationCommandMessage 
            (safe as long as it's only for transitions with that message type,
            any thing else doesn't make sense)
         */
        public bool thisCommand(Message msg, object obj)
        {
            AnimationCommandMessage m = msg as AnimationCommandMessage;
            return command == m.command;
        }
    }
}
