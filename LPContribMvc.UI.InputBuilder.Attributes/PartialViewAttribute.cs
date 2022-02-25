using System.ComponentModel.DataAnnotations;

namespace LPContribMvc.UI.InputBuilder.Attributes
{
	public class PartialViewAttribute : UIHintAttribute
	{
		public string PartialView
		{
			get;
			private set;
		}

		public PartialViewAttribute(string partialView)
			: base(partialView)
		{
			PartialView = partialView;
		}
	}
}
