using System;
using System.Collections;
using System.Collections.Generic;

namespace LPContribMvc.PortableAreas
{
	public interface IApplicationBus : IList<Type>, ICollection<Type>, IEnumerable<Type>, IEnumerable
	{
		void Send(IEventMessage eventMessage);

		void SetMessageHandlerFactory(IMessageHandlerFactory factory);
	}
}
