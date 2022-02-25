using System.Reflection;

namespace LPContribMvc.UI.InputBuilder.Conventions
{
	public interface IPropertyViewModelNameConvention
	{
		string PropertyName(PropertyInfo propertyInfo);
	}
}
