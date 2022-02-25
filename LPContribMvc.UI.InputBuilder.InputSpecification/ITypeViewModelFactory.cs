using LPContribMvc.UI.InputBuilder.Views;
using System;

namespace LPContribMvc.UI.InputBuilder.InputSpecification
{
	public interface ITypeViewModelFactory
	{
		bool CanHandle(Type type);

		TypeViewModel Create(Type type);
	}
}
