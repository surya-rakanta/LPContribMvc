using System.ComponentModel.DataAnnotations;

namespace LPContribMvc.UI.InputBuilder.Attributes
{
	public class ExampleAttribute : ValidationAttribute
	{
		public string Example
		{
			get;
			set;
		}

		public ExampleAttribute(string example)
		{
			Example = example;
		}

		public override bool IsValid(object value)
		{
			return true;
		}
	}
}
