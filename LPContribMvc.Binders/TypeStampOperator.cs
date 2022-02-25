using System;
using System.Web.Mvc;

namespace LPContribMvc.Binders
{
	public class TypeStampOperator : ITypeStampOperator
	{
		public string DetectTypeStamp(ModelBindingContext bindingContext, IPropertyNameProvider propertyNameProvider)
		{
			string text = propertyNameProvider.CreatePropertyName(bindingContext.ModelName, DerivedTypeModelBinderCache.TypeStampFieldName);
			if (bindingContext.ValueProvider.ContainsPrefix(text))
			{
				ValueProviderResult value = bindingContext.ValueProvider.GetValue(text);
				if (value.RawValue is string[])
				{
					return (value.RawValue as string[])[0];
				}
				throw new InvalidOperationException($"TypeStamp found for type {bindingContext.ModelType.Name} on path {text}, but format is invalid.");
			}
			return string.Empty;
		}
	}
}
