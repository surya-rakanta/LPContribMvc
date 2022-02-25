using LPContribMvc.UI.InputBuilder.Views;
using System;
using System.Reflection;

namespace LPContribMvc.UI.InputBuilder.InputSpecification
{
	public interface IViewModelFactory
	{
		TypeViewModel Create(Type type);

		PropertyViewModel Create(PropertyInfo propertyInfo, string name, bool indexed, Type type, object model);
	}
}
