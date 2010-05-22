using System;
using System.Collections.Generic;

namespace Gdd.Game.Engine.AI
{
    public class PriorityQueue<P, T> : List<T> where P : IComparable<P>
    {
        public delegate P priorityExtractor(T elem);

        public priorityExtractor priority;

        public T Peek() 
        { 
            if (this.Count == 0) return default(T); 
            return this[0];
        }

        public void Enqueue(T elem) 
        {
            this.Add(elem);
            fixUp(this.Count - 1);
        }

        public T Dequeue() 
        {
            T ret = this.Peek();
            if (this.Count > 0) {
                this[0] = this[this.Count - 1];
                this.RemoveAt(this.Count - 1);
                fixDown(0);
            }
            return ret;
        }

        public void fixHeap()
        {
            for (int i = 0; i < this.Count; i++) 
            {
                fixUp(i);
            }
        
        }

        protected void fixUp(int position)
        {
            if (position == 0) return;
            int i = position;
            while (true)
            {
                if ((i+1)/2 == 0)
                {
                    break;
                }
                P down = priority(this[i]);
                P up = priority(this[((i - 1) / 2)]);
                if (up.CompareTo(down) < 0)
                {
                    T tmp = this[i];
                    this[i] = this[((i - 1) / 2)];
                    this[((i - 1) / 2)] = tmp;
                    i = ((i - 1) / 2);
                    continue;
                }
                break;
            }
        
        }

        protected void fixDown(int position)
        { 
            int i = position;
            while (true) 
            { 
                int i1 = (i+1)*2-1;
                if(  i1+1 > this.Count ) 
                {
                    break;
                }
                int i2 = (i + 1) * 2;
                P up = priority(this[i]);
                int i0;
                P down;
                if (i2 + 1 > this.Count)
                {
                    i0 = i1;
                    down = priority(this[i1]);
                }
                else
                {
                    P down1 = priority(this[i1]);
                    P down2 = priority(this[i2]);
                    if (down1.CompareTo(down2) > 0)
                    {
                        down = down1;
                        i0 = i1;
                    }
                    else 
                    {
                        down = down2;
                        i0 = i2;
                    }
                }
                if (up.CompareTo(down) < 0) 
                {
                    T tmp = this[i];
                    this[i] = this[i0];
                    this[i0] = tmp;
                    i = i0;
                    continue;
                }
                break;
            }
        }

        public PriorityQueue() : base() {}
        public PriorityQueue(int capacity) : base(capacity) {}
        public PriorityQueue(IEnumerable<T> collection)
            : base(collection)
        {
            fixHeap();
        }
    
    }
}
