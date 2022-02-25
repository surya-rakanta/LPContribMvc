using LPContribMvc.UI.InputBuilder.Views;

namespace LPContribMvc.UI.InputBuilder
{
	public static class ArrayTypeViewModelExtensions
	{
		public static bool HasDeleteButton(this TypeViewModel model)
		{
			if (model.AdditionalValues.ContainsKey("hidedeletebutton"))
			{
				return !(bool)model.AdditionalValues["hidedeletebutton"];
			}
			return true;
		}

		public static bool HasAddButton(this TypeViewModel model)
		{
			if (model.AdditionalValues.ContainsKey("hideaddbutton"))
			{
				return !(bool)model.AdditionalValues["hideaddbutton"];
			}
			return true;
		}
	}
}
