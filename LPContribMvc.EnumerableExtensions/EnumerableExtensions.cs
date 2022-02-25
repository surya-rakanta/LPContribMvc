using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LPContribMvc.EnumerableExtensions
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<SelectListItem> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector)
		{
			return items.ToSelectList(valueSelector, nameSelector, (TItem x) => false);
		}

		public static IEnumerable<SelectListItem> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector, IEnumerable<TValue> selectedItems)
		{
			return items.ToSelectList(valueSelector, nameSelector, (TItem x) => selectedItems != null && selectedItems.Contains(valueSelector(x)));
		}

		public static IEnumerable<SelectListItem> ToSelectList<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> valueSelector, Func<TItem, string> nameSelector, Func<TItem, bool> selectedValueSelector)
		{

			foreach (var item in items)
			{
				var value = valueSelector(item);

				yield return new SelectListItem
				{
					Text = nameSelector(item),
					Value = value.ToString(),
					Selected = selectedValueSelector(item)
				};
			}

		}

	}
}
