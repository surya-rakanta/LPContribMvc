using LPContribMvc.UI.InputBuilder.Attributes;
using LPContribMvc.UI.InputBuilder.Conventions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LPContribMvc.UI.InputBuilder.Helpers
{
	public static class DisplayOrderExtensions
	{
		public static PropertyInfo[] ReOrderProperties(this PropertyInfo[] properties)
		{
			Dictionary<int, PropertyInfo> dictionary = new Dictionary<int, PropertyInfo>();
			List<PropertyInfo> list = new List<PropertyInfo>();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.AttributeExists<DisplayOrderAttribute>())
				{
					int order = propertyInfo.GetAttribute<DisplayOrderAttribute>().Order;
					dictionary.Add(order, propertyInfo);
				}
				else
				{
					list.Add(propertyInfo);
				}
			}
			List<PropertyInfo> list2 = new List<PropertyInfo>();
			foreach (KeyValuePair<int, PropertyInfo> item in dictionary.OrderBy((KeyValuePair<int, PropertyInfo> x) => x.Key).ToList())
			{
				list2.Add(item.Value);
			}
			foreach (PropertyInfo item2 in list.ToList())
			{
				list2.Add(item2);
			}
			return list2.ToArray();
		}
	}
}
