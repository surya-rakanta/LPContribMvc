using LPContribMvc.UI.InputBuilder.Attributes;
using LPContribMvc.UI.InputBuilder.Helpers;
using LPContribMvc.UI.InputBuilder.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Mvc;

namespace LPContribMvc.UI.InputBuilder.Conventions
{
	public class DefaultPropertyConvention : IPropertyViewModelFactory
	{
		public virtual bool CanHandle(PropertyInfo propertyInfo)
		{
			return true;
		}

		public virtual PropertyViewModel Create(PropertyInfo propertyInfo, object model, string name, Type type)
		{
			PropertyViewModel propertyViewModel = CreateViewModel<object>();
			propertyViewModel.PartialName = PartialNameConvention(propertyInfo);
			propertyViewModel.Type = type;
			propertyViewModel.Example = ExampleForPropertyConvention(propertyInfo);
			propertyViewModel.Label = LabelForPropertyConvention(propertyInfo);
			propertyViewModel.PropertyIsRequired = PropertyIsRequiredConvention(propertyInfo);
			propertyViewModel.Layout = Layout(propertyInfo);
			propertyViewModel.Value = ValueFromModelPropertyConvention(propertyInfo, model, name);
			propertyViewModel.Name = name;
			return propertyViewModel;
		}

		public virtual string Layout(PropertyInfo propertyInfo)
		{
			return "Field";
		}

		public virtual PropertyViewModel CreateViewModel<T>()
		{
			return new PropertyViewModel<T>();
		}

		public virtual string ExampleForPropertyConvention(PropertyInfo propertyInfo)
		{
			if (propertyInfo.AttributeExists<ExampleAttribute>())
			{
				return propertyInfo.GetAttribute<ExampleAttribute>().Example;
			}
			return string.Empty;
		}

		public virtual string LabelForPropertyConvention(PropertyInfo propertyInfo)
		{
			if (propertyInfo.AttributeExists<LabelAttribute>())
			{
				return propertyInfo.GetAttribute<LabelAttribute>().Label;
			}
			if (propertyInfo.AttributeExists<DisplayNameAttribute>())
			{
				return propertyInfo.GetAttribute<DisplayNameAttribute>().DisplayName;
			}
			return propertyInfo.Name.ToSeparatedWords();
		}

		public virtual bool PropertyIsRequiredConvention(PropertyInfo propertyInfo)
		{
			return propertyInfo.AttributeExists<RequiredAttribute>();
		}

		public virtual bool ModelIsInvalidConvention<T>(PropertyInfo propertyInfo, HtmlHelper<T> htmlHelper) where T : class
		{
			if (((ViewDataDictionary)htmlHelper.ViewData).ModelState.ContainsKey(propertyInfo.Name))
			{
				return ((Collection<ModelError>)(object)((ViewDataDictionary)htmlHelper.ViewData).ModelState[propertyInfo.Name].Errors).Count > 0;
			}
			return false;
		}

		public virtual object ValueFromModelPropertyConvention(PropertyInfo propertyInfo, object model, string parentName)
		{
			if (model != null)
			{
				object obj = null;
				try
				{
					obj = propertyInfo.GetValue(model, new object[0]);
				}
				catch
				{
				}
				if (obj != null)
				{
					return obj;
				}
			}
			return string.Empty;
		}

		public virtual string PartialNameConvention(PropertyInfo propertyInfo)
		{
			if (propertyInfo.AttributeExists<UIHintAttribute>())
			{
				return propertyInfo.GetAttribute<UIHintAttribute>().UIHint;
			}
			if (propertyInfo.AttributeExists<DataTypeAttribute>())
			{
				return propertyInfo.GetAttribute<DataTypeAttribute>().DataType.ToString();
			}
			return propertyInfo.PropertyType.Name;
		}
	}
}
