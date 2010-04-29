
using System;

namespace Gdd.Game.Engine.AI
{
	public class MessageTypes
	{
		public const int toAction = 0;
		public const int toIdle = 1;
        public const int animationCommand = 2;
        public const int characterPosition = 3;
        public const int die = 4;
        public const int resurect = 5;
        public const int toNextSetpoint = 6;
	} 
		
	public class Message
	{
		public int MessageType;
		public double timeGenerated;
		public double timeDelivery;
		public IMessageProcessor from;
		public IMessageProcessor to;
		
	}
	
	
}
