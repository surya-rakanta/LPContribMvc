using System;
using System.Collections;
using System.Collections.Generic;

namespace LPContribMvc.PortableAreas
{
	public class ApplicationBus : List<Type>, IApplicationBus, IList<Type>, ICollection<Type>, IEnumerable<Type>, IEnumerable
	{
		private IMessageHandlerFactory _factory;

		public new void Add(Type type)
		{
			if (type.GetInterface(typeof(IMessageHandler).Name) == null)
			{
				throw new InvalidOperationException($"Type {type.Name} must implement the IMessageHandler interface");
			}
			base.Add(type);
		}

		public ApplicationBus(IMessageHandlerFactory factory)
		{
			_factory = factory;
		}

		public void Send(IEventMessage eventMessage)
		{
			foreach (IMessageHandler item in GetHandlersForType(eventMessage.GetType()))
			{
				item.Handle(eventMessage);
			}
		}

		public void SetMessageHandlerFactory(IMessageHandlerFactory factory)
		{
			_factory = factory;
		}

		public IEnumerable<IMessageHandler> GetHandlersForType(Type type)
		{
			using (Enumerator enumerator = GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Type handlerType = enumerator.Current;
					IMessageHandler handler = _factory.Create(handlerType);
					if (handler.CanHandle(type))
					{
						yield return handler;
					}
				}
			}
		}
	}
}
