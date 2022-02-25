using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace LPContribMvc.UI.MenuBuilder
{
	public class MenuList : MenuItem, ICollection<MenuItem>, IEnumerable<MenuItem>, IEnumerable
	{
		private readonly List<MenuItem> items;

		public string ListClass
		{
			get;
			set;
		}

		public int Count => items.Count;

		public bool IsReadOnly => false;

		public MenuItem this[int i] => items[i];

		protected virtual bool IsRootList
		{
			get
			{
				if (base.Title == null)
				{
					return base.Icon == null;
				}
				return false;
			}
		}

		public MenuList()
		{
			items = new List<MenuItem>();
			ListClass = "";
		}

		public void Add(MenuItem item)
		{
			items.Add(item);
		}

		public void Clear()
		{
			items.Clear();
		}

		public bool Contains(MenuItem item)
		{
			return items.Contains(item);
		}

		public void CopyTo(MenuItem[] array, int arrayIndex)
		{
			items.CopyTo(array, arrayIndex);
		}

		public bool Remove(MenuItem item)
		{
			return items.Remove(item);
		}

		public IEnumerator<MenuItem> GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		protected virtual string RenderItems()
		{
			if (items.Count <= 0)
			{
				return string.Empty;
			}
			CleanTagBuilder cleanTagBuilder = new CleanTagBuilder("ul");
			((TagBuilder)cleanTagBuilder).AddCssClass(ListClass);
			foreach (MenuItem item in items)
			{
				((TagBuilder)cleanTagBuilder).InnerHtml = ((TagBuilder)cleanTagBuilder).InnerHtml + item.RenderHtml();
			}
			return cleanTagBuilder.ToString((TagRenderMode)0);
		}

		public override string RenderHtml()
		{
			if (!base.Prepared)
			{
				throw new InvalidOperationException("Must call Prepare before RenderHtml(TextWriter) or call RenderHtml(RequestContext, TextWriter)");
			}
			if (base.HideItem)
			{
				return string.Empty;
			}
			if (!IsRootList && HasSingleRenderableItem())
			{
				return items[0].RenderHtml();
			}
			if (IsRootList)
			{
				return RenderItems();
			}
			CleanTagBuilder cleanTagBuilder = new CleanTagBuilder("li");
			((TagBuilder)cleanTagBuilder).AddCssClass(base.ItemClass);
			((TagBuilder)cleanTagBuilder).InnerHtml = RenderLink() + RenderItems();
			return cleanTagBuilder.ToString(TagRenderMode.Normal);
		}

		protected bool HasSingleRenderableItem()
		{
			int num = 0;
			for (int i = 0; i < items.Count; i++)
			{
				if (num >= 2)
				{
					break;
				}
				MenuItem menuItem = items[i];
				if (!menuItem.HideItem)
				{
					num++;
				}
			}
			return num == 1;
		}

		public MenuList SetListClass(string listClass)
		{
			ListClass = listClass;
			return this;
		}

		public override void Prepare(ControllerContext controllerContext)
		{
			List<MenuItem> list = new List<MenuItem>(items);
			foreach (MenuItem item in list)
			{
				item.Prepare(controllerContext);
			}
			if (items.Count <= 0)
			{
				base.Disabled = true;
			}
			base.Prepared = true;
		}
	}
}
