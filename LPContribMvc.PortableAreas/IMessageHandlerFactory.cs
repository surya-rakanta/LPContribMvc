using System;

namespace LPContribMvc.PortableAreas
{
	public interface IMessageHandlerFactory
	{
		IMessageHandler Create(Type type);
	}
}
