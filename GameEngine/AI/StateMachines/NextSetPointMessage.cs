using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gdd.Game.Engine.AI.StateMachines
{
    class NextSetPointMessage : Message
    {
        public NextSetPointMessage(IMessageProcessor _to,IMessageProcessor _from)
        {
            MessageType = MessageTypes.toNextSetpoint;
            to = _to;
            from = _from;
            timeDelivery = 0; // ASAP
        }
    }
}
