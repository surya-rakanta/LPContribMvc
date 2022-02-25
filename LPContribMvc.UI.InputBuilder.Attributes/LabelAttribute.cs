using System.ComponentModel.DataAnnotations;

namespace LPContribMvc.UI.InputBuilder.Attributes
{
	public class LabelAttribute : ValidationAttribute
	{
		public string Label
		{
			get;
			private set;
		}

		public LabelAttribute(string label)
		{
			Label = label;
		}

		public override bool IsValid(object value)
		{
			return true;
		}
	}
}
