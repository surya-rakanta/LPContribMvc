using LPContribMvc.PortableAreas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LPContribMvc
{
	public class Bus
	{
		private static IApplicationBus _instance;

		private static object _busLock = new object();

		public static IApplicationBus Instance
		{
			get
			{
				InitializeTheDefaultBus();
				return _instance;
			}
			set
			{
				_instance = value;
			}
		}

		private static void InitializeTheDefaultBus()
		{
			if (_instance == null)
			{
				lock (_busLock)
				{
					if (_instance == null)
					{
						_instance = new ApplicationBus(new MessageHandlerFactory());
						AddAllMessageHandlers();
					}
				}
			}
		}

		public static void Send(IEventMessage eventMessage)
		{
			Instance.Send(eventMessage);
		}

		public static void AddMessageHandler(Type type)
		{
			Instance.Add(type);
		}

		public static void AddAllMessageHandlers()
		{
			IEnumerable<Type> enumerable = FindAllMessageHandlers();
			foreach (Type item in enumerable)
			{
				Instance.Add(item);
			}
		}

		private static IEnumerable<Type> FindAllMessageHandlers()
		{
			IEnumerable<Type> enumerable = Type.EmptyTypes;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				Type[] types;
				try
				{
					types = assembly.GetTypes();
				}
				catch (ReflectionTypeLoadException ex)
				{
					types = ex.Types;
				}
				enumerable = enumerable.Concat(types);
			}
			return enumerable.Where((Type type) => IsValidType(type));
		}

		public static bool IsValidType(Type type)
		{
			if (type == null || type.IsInterface || type.IsAbstract || type.IsNestedPrivate)
			{
				return false;
			}
			return type.GetInterface(typeof(IMessageHandler).Name) != null;
		}
	}
}
