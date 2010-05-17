
using System;
using System.Collections.Generic;

namespace Gdd.Game.Engine.AI
{
	
	
	public class State : IState
	{
		protected string name;
		protected object thisObject;
		static uint nrUnnamedStates = 0;
		Dictionary<int,List<Transition>> transitions = new Dictionary<int,List<Transition>>(2);
		
		
		public State()
		{
			name = "UnnamedState" + ++nrUnnamedStates;
		}
		
		public State(string name)
		{
			this.name = name;
		}
		
		public void setThisObject(object obj)
		{
			thisObject = obj;
		}
		
		public string getName(){return name;}
		
		public IState checkTransitions(Message msg){
			if(transitions.ContainsKey(msg.MessageType))
			{
				foreach( Transition t in transitions[msg.MessageType]){
					if(t.transitionTest(msg, thisObject))
					{
						return t.nextState;
					}
				}
			}
			
			return this;
		}
		
		public void addTransition(int messageType, Transition t)
		{
			if(!transitions.ContainsKey(messageType)){
				transitions.Add(messageType,new List<Transition>(1));
			}
			transitions[messageType].Add(t);
		}
		
		public virtual void OnEnter(){
            IDebugable thisDebugable = thisObject as IDebugable;
            if (thisDebugable != null) if (thisDebugable.Debug)
            {
                Console.WriteLine(name + ".OnEnter()");
            }
        }

        public virtual void Update(double timeDiff)
        {
            IDebugable thisDebugable = thisObject as IDebugable;
            if (thisDebugable != null) if (thisDebugable.Debug)
                {
                    Console.WriteLine(name + ".Update()");
                }
        }
		
		public virtual void OnExit(){
            IDebugable thisDebugable = thisObject as IDebugable;
            if (thisDebugable != null) if (thisDebugable.Debug)
                {
                    Console.WriteLine(name + ".OnExit()");
                }
        }
		
		public virtual bool ProcessMessage(Message m){
            IDebugable thisDebugable = thisObject as IDebugable;
            if (thisDebugable != null) if (thisDebugable.Debug)
                {
                    Console.WriteLine(name + ".ProcessMessage({0})", m.MessageType);
                }
            return true;
        }
	}
}
