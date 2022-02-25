using System;

namespace LPContribMvc.UI.MenuBuilder
{
	public class MenuHelpText : Attribute
	{
		public string HelpText
		{
			get;
			set;
		}

		public MenuHelpText(string helpText)
		{
			HelpText = helpText;
		}
	}
}
