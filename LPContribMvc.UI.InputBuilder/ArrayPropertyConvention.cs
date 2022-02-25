using LPContribMvc.UI.InputBuilder.Attributes;
using LPContribMvc.UI.InputBuilder.Conventions;
using LPContribMvc.UI.InputBuilder.InputSpecification;
using LPContribMvc.UI.InputBuilder.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LPContribMvc.UI.InputBuilder
{
	public class ArrayPropertyConvention : DefaultPropertyConvention, IRequireViewModelFactory
	{
		public const string HIDE_ADD_BUTTON = "hideaddbutton";

		public const string HIDE_DELETE_BUTTON = "hidedeletebutton";

		public const string CAN_DELETE_ALL = "candeleteall";

		private IViewModelFactory _factory;

		public override bool CanHandle(PropertyInfo propertyInfo)
		{
			return propertyInfo.PropertyType.IsArray;
		}

		public override object ValueFromModelPropertyConvention(PropertyInfo propertyInfo, object model, string parentName)
		{
			List<TypeViewModel> list = new List<TypeViewModel>();
			object obj = base.ValueFromModelPropertyConvention(propertyInfo, model, parentName);
			int num = 0;
			foreach (object item in (IEnumerable)obj)
			{
				TypeViewModel typeViewModel = _factory.Create(item.GetType());
				typeViewModel.Value = item;
				typeViewModel.Name = parentName + "[" + num + "]";
				typeViewModel.PartialName = "Subform";
				typeViewModel.Properties = GetProperies(item, typeViewModel.Name + ".").ToArray();
				typeViewModel.Layout = "";
				typeViewModel.Index = num;
				list.Add(typeViewModel);
				num++;
			}
			return list;
		}

		private IEnumerable<PropertyViewModel> GetProperies(object o, string parentName)
		{
			try
			{
				PropertyInfo[] properties = o.GetType().GetProperties();
				foreach (PropertyInfo info in properties)
				{
					PropertyViewModel properies = _factory.Create(info, parentName + info.Name, indexed: false, o.GetType(), o);
					properies.Layout = "Row";
					yield return properies;
				}
			}
			finally
			{
			}
		}

		public override string Layout(PropertyInfo propertyInfo)
		{
			return "Array";
		}

		public override string PartialNameConvention(PropertyInfo propertyInfo)
		{
			return "Array";
		}

		public override PropertyViewModel CreateViewModel<T>()
		{
			return base.CreateViewModel<IEnumerable<TypeViewModel>>();
		}

		public void Set(IViewModelFactory factory)
		{
			_factory = factory;
		}

		public override PropertyViewModel Create(PropertyInfo propertyInfo, object model, string name, Type type)
		{
			PropertyViewModel propertyViewModel = base.Create(propertyInfo, model, name, type);
			if (propertyInfo.AttributeExists<NoAddAttribute>())
			{
				propertyViewModel.AdditionalValues.Add("hideaddbutton", true);
			}
			if (propertyInfo.AttributeExists<CanDeleteAllAttribute>())
			{
				propertyViewModel.AdditionalValues.Add("candeleteall", true);
			}
			if (propertyInfo.AttributeExists<NoDeleteAttribute>())
			{
				propertyViewModel.AdditionalValues.Add("hidedeletebutton", true);
				{
					foreach (TypeViewModel item in (IEnumerable)propertyViewModel.Value)
					{
						item.AdditionalValues.Add("hidedeletebutton", true);
					}
					return propertyViewModel;
				}
			}
			return propertyViewModel;
		}
	}
}
