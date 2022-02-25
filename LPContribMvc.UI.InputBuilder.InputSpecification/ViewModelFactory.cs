using LPContribMvc.UI.InputBuilder.Conventions;
using LPContribMvc.UI.InputBuilder.Helpers;
using LPContribMvc.UI.InputBuilder.Views;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace LPContribMvc.UI.InputBuilder.InputSpecification
{
	public class ViewModelFactory<T> : IViewModelFactory where T : class
	{
		private readonly IPropertyViewModelFactory[] _propertyFactories;

		private readonly ITypeViewModelFactory[] _typeFactories;

		private readonly IPropertyViewModelNameConvention _nameConventions;

		private readonly HtmlHelper<T> _htmlHelper;

		public ViewModelFactory(HtmlHelper<T> htmlHelper, IPropertyViewModelFactory[] propertyFactories, IPropertyViewModelNameConvention nameConvention, ITypeViewModelFactory[] typeFactories)
		{
			_htmlHelper = htmlHelper;
			_typeFactories = typeFactories;
			_propertyFactories = propertyFactories;
			_nameConventions = nameConvention;
		}

		public PropertyViewModel Create(Expression<Func<T, object>> expression)
		{
			PropertyInfo propertyInfo = ReflectionHelper.FindPropertyFromExpression(expression);
			string name = ReflectionHelper.BuildNameFrom(expression);
			bool indexed = ReflectionHelper.IsIndexed(expression);
			return Create(propertyInfo, name, indexed, expression.Body.Type);
		}

		public PropertyViewModel Create(PropertyInfo propertyInfo, string parentName)
		{
			string name = parentName + _nameConventions.PropertyName(propertyInfo);
			return Create(propertyInfo, name, indexed: false, propertyInfo.PropertyType);
		}

		public virtual object ValueFromModelPropertyConvention(PropertyInfo propertyInfo, object model)
		{
			if (model != null)
			{
				object value = propertyInfo.GetValue(model, new object[0]);
				if (value != null)
				{
					return value;
				}
			}
			return string.Empty;
		}

		public virtual PropertyViewModel Create(PropertyInfo propertyInfo, string name, bool indexed, Type type)
		{
			return Create(propertyInfo, name, indexed, type, _htmlHelper.ViewData.Model);
		}

		public virtual PropertyViewModel Create(PropertyInfo propertyInfo, string name, bool indexed, Type type, object model)
		{
			IPropertyViewModelFactory[] propertyFactories = _propertyFactories;
			foreach (IPropertyViewModelFactory propertyViewModelFactory in propertyFactories)
			{
				if (propertyViewModelFactory.CanHandle(propertyInfo))
				{
					if (propertyViewModelFactory is IRequireViewModelFactory)
					{
						((IRequireViewModelFactory)propertyViewModelFactory).Set(this);
					}
					return propertyViewModelFactory.Create(propertyInfo, model, name, type);
				}
			}
			throw new InvalidOperationException("Could not find an Input Builder convention(IPropertyViewModelFactory) for type:" + propertyInfo.PropertyType + " and Name:" + name);
		}

		public TypeViewModel Create()
		{
			return Create(typeof(T));
		}

		public TypeViewModel Create(Type type)
		{
			ITypeViewModelFactory[] typeFactories = _typeFactories;
			foreach (ITypeViewModelFactory typeViewModelFactory in typeFactories)
			{
				if (typeViewModelFactory.CanHandle(type))
				{
					return typeViewModelFactory.Create(type);
				}
			}
			throw new InvalidOperationException("Could not find an Input Builder Type Convention(ITypeViewModelFactory) for type:" + type.Name);
		}
	}
}
