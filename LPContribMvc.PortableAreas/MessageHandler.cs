using System;

namespace LPContribMvc.PortableAreas
{
	public abstract class MessageHandler<TMessage> : IMessageHandler<TMessage>, IMessageHandler where TMessage : IEventMessage
	{
		public abstract void Handle(TMessage message);

		public virtual void Handle(object message)
		{
			Handle((TMessage)message);
		}

		public virtual bool CanHandle(Type type)
		{
			return type == typeof(TMessage);
		}
	}
}
