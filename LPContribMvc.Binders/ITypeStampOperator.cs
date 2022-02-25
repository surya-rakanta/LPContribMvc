using System.Web.Mvc;

namespace LPContribMvc.Binders
{
	public interface ITypeStampOperator
	{
		string DetectTypeStamp(ModelBindingContext bindingContext, IPropertyNameProvider propertyNameProvider);
	}
}
