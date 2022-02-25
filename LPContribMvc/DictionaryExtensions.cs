using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace LPContribMvc
{
	public static class DictionaryExtensions
	{
		public static IDictionary<string, T> Add<T>(this IDictionary<string, T> dict, params Func<object, T>[] hash)
		{
			if (dict == null || hash == null)
			{
				return dict;
			}
			foreach (Func<object, T> func in hash)
			{
				dict.Add(func.Method.GetParameters()[0].Name, func(null));
			}
			return dict;
		}

		public static IDictionary Add(this IDictionary dict, params Func<object, object>[] hash)
		{
			if (dict == null || hash == null)
			{
				return dict;
			}
			foreach (Func<object, object> func in hash)
			{
				dict.Add(func.Method.GetParameters()[0].Name, func(null));
			}
			return dict;
		}

		public static Dictionary<string, object> AnonymousObjectToCaseSensitiveDictionary(object objectToConvert)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.Ordinal);
			if (objectToConvert != null)
			{
				foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(objectToConvert))
				{
					dictionary[property.Name] = property.GetValue(objectToConvert);
				}
				return dictionary;
			}
			return dictionary;
		}

		public static string ToHtmlAttributes(IDictionary<string, object> attributes)
		{
			if (attributes == null || attributes.Count == 0)
			{
				return string.Empty;
			}
			IEnumerable<string> source = from pair in attributes
				let value = (pair.Value == null) ? null : HttpUtility.HtmlAttributeEncode(pair.Value.ToString())
				select $"{pair.Key}=\"{value}\"";
			return string.Join(" ", source.ToArray());
		}
	}
}
