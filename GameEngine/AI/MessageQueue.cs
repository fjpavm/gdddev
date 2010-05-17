using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gdd.Game.Engine.AI
{
    public class MessageQueue
    {
        List<Message> expected = new List<Message>();
        PriorityQueue<double, Message> queue = new PriorityQueue<double, Message>();
        double currentTime;
        
        public MessageQueue() 
        {
            queue.priority = x => -x.timeDelivery;
        }

        public void sendMessage(Message m) 
        {
            if (m.timeDelivery > currentTime)
            {
                queue.Enqueue(m);
            }
            else 
            {
                expected.Add(m);
            }
        }

        public void deliverMessages() 
        {
            for (int i = 0; i < expected.Count; i++ )
            {
                Message m = expected[i];
                m.to.ProcessMessage(m);
            }
            expected.Clear();

            while (queue.Peek() != null) 
            {
                if (queue.Peek().timeDelivery > currentTime) 
                {
                    break;
                }
                Message m = queue.Dequeue();
                m.to.ProcessMessage(m);
            }
        }

        public void updateCurrentTime(double t) 
        {
            currentTime = t;
        }
    }
}
