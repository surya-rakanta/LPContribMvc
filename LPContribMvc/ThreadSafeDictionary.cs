using System;
using System.Collections.Generic;
using System.Threading;

namespace LPContribMvc
{
	internal class ThreadSafeDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	{
		private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

		public TValue GetOrAdd(TKey key, Func<TValue> defaultValueDelegate)
		{
			cacheLock.EnterReadLock();
			try
			{
				if (ContainsKey(key))
				{
					return base[key];
				}
			}
			finally
			{
				cacheLock.ExitReadLock();
			}
			cacheLock.EnterWriteLock();
			try
			{
				if (!ContainsKey(key))
				{
					Add(key, defaultValueDelegate());
				}
				return base[key];
			}
			finally
			{
				cacheLock.ExitWriteLock();
			}
		}
	}
}
