using LPContribMvc.UI.InputBuilder.Views;
using System;
using System.Reflection;

namespace LPContribMvc.UI.InputBuilder.Conventions
{
	public class DateTimePropertyConvention : DefaultPropertyConvention
	{
		public override bool CanHandle(PropertyInfo propertyInfo)
		{
			return propertyInfo.PropertyType == typeof(DateTime);
		}

		public override PropertyViewModel CreateViewModel<T>()
		{
			return new PropertyViewModel<DateTime>();
		}
	}
}
