using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace LPContribMvc.UI.MenuBuilder
{
	[Obsolete]
	public static class Menu
	{
		public static string DefaultAnchorClass
		{
			get;
			set;
		}

		public static string DefaultIconClass
		{
			get;
			set;
		}

		public static string DefaultItemClass
		{
			get;
			set;
		}

		public static string DefaultListClass
		{
			get;
			set;
		}

		public static string DefaultIconDirectory
		{
			get;
			set;
		}

		public static bool DefaultShowWhenDisabled
		{
			get;
			set;
		}

		public static string DefaultDisabledClass
		{
			get;
			set;
		}

		public static string DefaultSelectedClass
		{
			get;
			set;
		}

		public static MenuList Begin(params MenuItem[] items)
		{
			return Items(null, null, items);
		}

		public static MenuList Items(string title, params MenuItem[] items)
		{
			return Items(title, null, items);
		}

		public static MenuList Items(string title, string icon, params MenuItem[] items)
		{
			MenuList menuList = new MenuList();
			menuList.Title = title;
			menuList.Icon = icon;
			MenuList menuList2 = menuList;
			foreach (MenuItem item in items)
			{
				menuList2.Add(item);
			}
			return (MenuList)AddDefaults(menuList2);
		}

		public static MenuItem Secure<T>(Expression<Action<T>> menuAction) where T : Controller
		{
			return Secure(menuAction, null, null);
		}

		public static MenuItem Secure<T>(Expression<Action<T>> menuAction, string title) where T : Controller
		{
			return Secure(menuAction, title, null);
		}

		public static MenuItem Secure<T>(Expression<Action<T>> menuAction, string title, string icon) where T : Controller
		{
			SecureActionMenuItem<T> secureActionMenuItem = new SecureActionMenuItem<T>();
			secureActionMenuItem.MenuAction = menuAction;
			secureActionMenuItem.Title = title;
			secureActionMenuItem.Icon = icon;
			return AddDefaults(secureActionMenuItem);
		}

		public static MenuItem Action<T>(Expression<Action<T>> menuAction) where T : Controller
		{
			return Action(menuAction, null, null);
		}

		public static MenuItem Action<T>(Expression<Action<T>> menuAction, string title) where T : Controller
		{
			return Action(menuAction, title, null);
		}

		public static MenuItem Action<T>(Expression<Action<T>> menuAction, string title, string icon) where T : Controller
		{
			ActionMenuItem<T> actionMenuItem = new ActionMenuItem<T>();
			actionMenuItem.MenuAction = menuAction;
			actionMenuItem.Title = title;
			actionMenuItem.Icon = icon;
			return AddDefaults(actionMenuItem);
		}

		public static MenuItem Link(string url, string title)
		{
			return Link(url, title, null);
		}

		public static MenuItem Link(string url, string title, string icon)
		{
			MenuItem menuItem = new MenuItem();
			menuItem.Title = title;
			menuItem.ActionUrl = url;
			menuItem.Icon = icon;
			return AddDefaults(menuItem);
		}

		private static MenuItem AddDefaults(MenuItem item)
		{
			item.AnchorClass = DefaultAnchorClass;
			item.IconClass = DefaultIconClass;
			item.ItemClass = DefaultItemClass;
			item.IconDirectory = DefaultIconDirectory;
			item.DisabledClass = DefaultDisabledClass;
			item.ShowWhenDisabled = DefaultShowWhenDisabled;
			item.SelectedClass = DefaultSelectedClass;
			if (item is MenuList)
			{
				((MenuList)item).ListClass = DefaultListClass;
			}
			return item;
		}
	}
}
