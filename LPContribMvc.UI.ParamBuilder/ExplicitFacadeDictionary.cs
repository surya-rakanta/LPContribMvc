using System.Collections;
using System.Collections.Generic;

namespace LPContribMvc.UI.ParamBuilder
{
	public abstract class ExplicitFacadeDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
	{
		protected abstract IDictionary<TKey, TValue> Wrapped
		{
			get;
		}

		ICollection<TKey> IDictionary<TKey, TValue>.Keys => Wrapped.Keys;

		ICollection<TValue> IDictionary<TKey, TValue>.Values => Wrapped.Values;

		TValue IDictionary<TKey, TValue>.this[TKey key]
		{
			get
			{
				return Wrapped[key];
			}
			set
			{
				Wrapped[key] = value;
			}
		}

		int ICollection<KeyValuePair<TKey, TValue>>.Count => Wrapped.Count;

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => Wrapped.IsReadOnly;

		void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
		{
			Wrapped.Add(key, value);
		}

		bool IDictionary<TKey, TValue>.ContainsKey(TKey key)
		{
			return Wrapped.ContainsKey(key);
		}

		bool IDictionary<TKey, TValue>.Remove(TKey key)
		{
			return Wrapped.Remove(key);
		}

		bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
		{
			return Wrapped.TryGetValue(key, out value);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
		{
			Wrapped.Add(item);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Clear()
		{
			Wrapped.Clear();
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
		{
			return Wrapped.Contains(item);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			Wrapped.CopyTo(array, arrayIndex);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
		{
			return Wrapped.Remove(item);
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return Wrapped.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)this).GetEnumerator();
		}
	}
}
