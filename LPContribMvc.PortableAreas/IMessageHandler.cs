using System;

namespace LPContribMvc.PortableAreas
{
	public interface IMessageHandler
	{
		void Handle(object message);

		bool CanHandle(Type type);
	}
	public interface IMessageHandler<TMessage> : IMessageHandler where TMessage : IEventMessage
	{
		void Handle(TMessage message);
	}
}
