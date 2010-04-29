using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gdd.Game.Engine.AI
{
    class AnimationCommandMessage : Message
    {
        public AnimationCommandMessage(IMessageProcessor _to,IMessageProcessor _from,int _command) 
        {
            MessageType = MessageTypes.animationCommand;
            command = _command;
            to = _to;
            from = _from;
            timeDelivery = 0; // ASAP
        }
        public int command;
    }
}
