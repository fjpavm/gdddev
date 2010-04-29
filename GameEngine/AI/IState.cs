
using System;

namespace Gdd.Game.Engine.AI
{
	public delegate bool Test(Message msg, object obj);
	public class Transition{
		public Test transitionTest;
		public IState nextState;
	}
	
	public interface IState : IMessageProcessor
	{
		void addTransition(int messageType, Transition t);
		IState checkTransitions(Message msg);
		void OnEnter();
		void Update();
		void OnExit();
        void setThisObject(object obj);
		
	}
}
