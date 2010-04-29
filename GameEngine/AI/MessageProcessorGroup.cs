
using System;
using System.Collections.Generic;

namespace Gdd.Game.Engine.AI
{
	
	
	public class MessageProcessorGroup : IMessageProcessor
	{
        LinkedList<IMessageProcessor> messageProcessors = new LinkedList<IMessageProcessor>();
		
		public MessageProcessorGroup()
		{
		}

        public void addMessageProcessor(IMessageProcessor mp)
        {
            messageProcessors.AddLast(mp);
        }
		
		public bool ProcessMessage(Message m)
        {
            bool ret = false;
            foreach (IMessageProcessor mp in messageProcessors) 
            {
                ret |= mp.ProcessMessage(m);
            }
            return ret;
        }
		
	}
}
