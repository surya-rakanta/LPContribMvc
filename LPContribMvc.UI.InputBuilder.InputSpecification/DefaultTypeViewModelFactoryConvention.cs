using LPContribMvc.UI.InputBuilder.Attributes;
using LPContribMvc.UI.InputBuilder.Conventions;
using LPContribMvc.UI.InputBuilder.Helpers;
using LPContribMvc.UI.InputBuilder.Views;
using System;

namespace LPContribMvc.UI.InputBuilder.InputSpecification
{
	public class DefaultTypeViewModelFactoryConvention : ITypeViewModelFactory
	{
		public bool CanHandle(Type type)
		{
			return true;
		}

		public TypeViewModel Create(Type type)
		{
			TypeViewModel typeViewModel = new TypeViewModel();
			typeViewModel.Label = LabelForTypeConvention(type);
			typeViewModel.PartialName = "Form";
			typeViewModel.Type = type;
			return typeViewModel;
		}

		public string LabelForTypeConvention(Type type)
		{
			if (type.AttributeExists<LabelAttribute>())
			{
				return type.GetAttribute<LabelAttribute>().Label;
			}
			return type.Name.ToSeparatedWords();
		}
	}
}
