
using System;
using System.Collections.Generic;

namespace Gdd.Game.Engine.AI
{
	
	public class MessageProcessorGroup : IMessageProcessor
	{
        List<IMessageProcessor> messageProcessors = new List<IMessageProcessor>();
		
		public MessageProcessorGroup()
		{
		}

        public void addMessageProcessor(IMessageProcessor mp)
        {
            messageProcessors.Add(mp);
        }

        public void removeMessageProcessor(IMessageProcessor mp)
        {
            messageProcessors.Remove(mp);
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
