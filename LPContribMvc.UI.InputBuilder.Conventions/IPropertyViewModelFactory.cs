using LPContribMvc.UI.InputBuilder.Views;
using System;
using System.Reflection;

namespace LPContribMvc.UI.InputBuilder.Conventions
{
	public interface IPropertyViewModelFactory
	{
		bool CanHandle(PropertyInfo propertyInfo);

		PropertyViewModel Create(PropertyInfo propertyInfo, object model, string name, Type type);
	}
}
