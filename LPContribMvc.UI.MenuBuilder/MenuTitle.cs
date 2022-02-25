using System;

namespace LPContribMvc.UI.MenuBuilder
{
	public class MenuTitle : Attribute
	{
		public string Title
		{
			get;
			set;
		}

		public MenuTitle(string title)
		{
			Title = title;
		}
	}
}
