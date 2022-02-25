using System;
using System.Collections.Generic;

namespace LPContribMvc
{
	public class Hash<TValue> : Dictionary<string, TValue>
	{
		public static Dictionary<string, TValue> Empty => new Dictionary<string, TValue>(0, StringComparer.OrdinalIgnoreCase);

		public Hash(params Func<object, TValue>[] hash)
			: base((hash != null) ? hash.Length : 0, (IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase)
		{
			if (hash != null)
			{
				foreach (Func<object, TValue> func in hash)
				{
					Add(func.Method.GetParameters()[0].Name, func(null));
				}
			}
		}
	}
	public class Hash : Hash<object>
	{
		public Hash(params Func<object, object>[] hash)
			: base(hash)
		{
		}
	}
}
