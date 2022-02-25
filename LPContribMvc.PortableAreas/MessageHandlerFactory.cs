using System;

namespace LPContribMvc.PortableAreas
{
	public class MessageHandlerFactory : IMessageHandlerFactory
	{
		public IMessageHandler Create(Type type)
		{
			return (IMessageHandler)Activator.CreateInstance(type);
		}
	}
}
