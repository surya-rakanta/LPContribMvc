using System;
using System.Linq;
using System.Reflection;

namespace LPContribMvc.UI.InputBuilder.Conventions
{
	public static class PropertyInfoExtensions
	{
		public static bool AttributeExists<T>(this PropertyInfo propertyInfo) where T : class
		{
			T val = propertyInfo.GetCustomAttributes(typeof(T), inherit: false).FirstOrDefault() as T;
			if (val == null)
			{
				return false;
			}
			return true;
		}

		public static bool AttributeExists<T>(this Type type) where T : class
		{
			T val = type.GetCustomAttributes(typeof(T), inherit: false).FirstOrDefault() as T;
			if (val == null)
			{
				return false;
			}
			return true;
		}

		public static T GetAttribute<T>(this Type type) where T : class
		{
			return type.GetCustomAttributes(typeof(T), inherit: false).FirstOrDefault() as T;
		}

		public static T GetAttribute<T>(this PropertyInfo propertyInfo) where T : class
		{
			return propertyInfo.GetCustomAttributes(typeof(T), inherit: false).FirstOrDefault() as T;
		}
	}
}
