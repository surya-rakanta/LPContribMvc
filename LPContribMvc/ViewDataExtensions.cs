using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LPContribMvc
{
	[Obsolete]
	public static class ViewDataExtensions
	{
		public static IDictionary<string, object> Add<T>(this IDictionary<string, object> bag, T anObject)
		{
			Type typeFromHandle = typeof(T);
			if (bag.ContainsKey(getKey(typeFromHandle)))
			{
				string message = $"You can only add one default object for type '{typeFromHandle}'.";
				throw new ArgumentException(message);
			}
			bag.Add(getKey(typeFromHandle), anObject);
			return bag;
		}

		public static IDictionary<string, object> Add<T>(this IDictionary<string, object> bag, string key, T value)
		{
			bag.Add(key, value);
			return bag;
		}

		public static T Get<T>(this IDictionary<string, object> bag)
		{
			return bag.Get<T>(getKey(typeof(T)));
		}

		public static T GetOrDefault<T>(this IDictionary<string, object> bag, string key, T defaultValue)
		{
			if (bag.ContainsKey(key))
			{
				return (T)bag[key];
			}
			return defaultValue;
		}

		public static object Get(this IDictionary<string, object> bag, Type type)
		{
			if (!bag.ContainsKey(getKey(type)))
			{
				string message = $"No object exists that is of type '{type}'.";
				throw new ArgumentException(message);
			}
			return bag[getKey(type)];
		}

		private static string getKey(Type type)
		{
			return type.FullName;
		}

		public static bool Contains<T>(this IDictionary<string, object> bag)
		{
			return bag.ContainsKey(getKey(typeof(T)));
		}

		public static bool Contains(this IDictionary<string, object> bag, Type keyType)
		{
			return bag.ContainsKey(getKey(keyType));
		}

		public static T Get<T>(this IDictionary<string, object> bag, string key)
		{
			if (!bag.ContainsKey(key))
			{
				string message = $"No object exists with key '{key}'.";
				throw new ArgumentException(message);
			}
			return (T)bag[key];
		}

		public static int GetCount(this IDictionary<string, object> bag, Type type)
		{
			int num = 0;
			foreach (object value in bag.Values)
			{
				if (type.Equals(value.GetType()))
				{
					num++;
				}
			}
			return num;
		}

		public static T Get<T>(this ViewDataDictionary bag)
		{
			return bag.Get<T>(getKey(typeof(T)));
		}

		public static T GetOrDefault<T>(this ViewDataDictionary bag, string key, T defaultValue)
		{
			if (bag.ContainsKey(key))
			{
				return (T)bag[key];
			}
			return defaultValue;
		}

		public static object Get(this ViewDataDictionary bag, Type type)
		{
			if (!bag.ContainsKey(getKey(type)))
			{
				string message = $"No object exists that is of type '{type}'.";
				throw new ArgumentException(message);
			}
			return bag[getKey(type)];
		}

		public static bool Contains<T>(this ViewDataDictionary bag)
		{
			return bag.ContainsKey(getKey(typeof(T)));
		}

		public static bool Contains(this ViewDataDictionary bag, Type keyType)
		{
			return bag.ContainsKey(getKey(keyType));
		}

		public static bool Contains(this ViewDataDictionary bag, string key)
		{
			return bag.ContainsKey(key);
		}

		public static T Get<T>(this ViewDataDictionary bag, string key)
		{
			if (!bag.ContainsKey(key))
			{
				string message = $"No object exists with key '{key}'.";
				throw new ArgumentException(message);
			}
			return (T)bag[key];
		}
	}
}
