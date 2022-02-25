using System;

namespace LPContribMvc.UI.InputBuilder.Attributes
{
	public class DisplayOrderAttribute : Attribute
	{
		public int Order
		{
			get;
			set;
		}

		public DisplayOrderAttribute(int order)
		{
			Order = order;
		}
	}
}
