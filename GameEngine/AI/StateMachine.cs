
using System;
using System.Collections.Generic;

namespace Gdd.Game.Engine.AI
{
	
	
	public class StateMachine : State
	{
		static uint nrUnnamedStateMachines = 0;
        static State dummyState = new State("DummyState");
        IState currentState = dummyState;
		// maybe don't need dictionary in state machine
		// Dictionary<string,IState> stateTable;
        IState nextState = dummyState;
        IState initialState = dummyState;

        public IState InitialState
        {
            set
            {
                initialState = value;
            }
        }
		
		public StateMachine() : base("UnnamedStateMachine" + ++nrUnnamedStateMachines)
		{
		}
		
		public StateMachine(IState initialState, string name) : base(name)
		{
			this.initialState = initialState;
		}

        public StateMachine(string name)
            : base(name)
        {
        }
		
		public override void OnEnter()
		{
            base.OnEnter();
			currentState = initialState;
			nextState = initialState;
			currentState.OnEnter();
		}
		
		public override void Update()
		{
            base.Update();
            // In case it's not started yet
            if (currentState == dummyState) 
            {
                nextState = initialState;
            }
			if(nextState != currentState)
			{
				currentState.OnExit();
				nextState.OnEnter();
				currentState = nextState;
			}else{
				currentState.Update();
			}
		}
		
		public override void OnExit()
		{
            base.OnExit();
			currentState.OnExit();
		}
		
		public override bool ProcessMessage(Message m){
            base.ProcessMessage(m);
            if (currentState == nextState)
            {
                nextState = currentState.checkTransitions(m);
            }
			if(currentState == nextState)
			{
				currentState.ProcessMessage(m);
			}
			return true;
		}
	}
}
