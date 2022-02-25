using System;
using System.Reflection;

namespace LPContribMvc.UI.InputBuilder.Conventions
{
	public class GuidPropertyConvention : DefaultPropertyConvention, IPropertyViewModelFactory
	{
		public override bool CanHandle(PropertyInfo propertyInfo)
		{
			return propertyInfo.PropertyType.IsAssignableFrom(typeof(Guid));
		}

		public override string Layout(PropertyInfo info)
		{
			return "HiddenField";
		}

		public override string PartialNameConvention(PropertyInfo propertyInfo)
		{
			return "Guid";
		}
	}
}
