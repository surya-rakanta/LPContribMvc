using System;
using System.Web.Mvc;

namespace LPContribMvc.Binders
{
	public class DerivedTypeModelBinder : DefaultModelBinder, IPropertyNameProvider
	{
		private readonly ITypeStampOperator _typeStampOperator;

		public DerivedTypeModelBinder()
			: this(null)
		{
		}

		public DerivedTypeModelBinder(ITypeStampOperator typeStampOperator) : base()
		{
			_typeStampOperator = (typeStampOperator ?? new TypeStampOperator());
		}

		protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
		{
			Type type = DetectInstantiationType(controllerContext, bindingContext, modelType);
			if (type == modelType)
			{
				return base.CreateModel(controllerContext, bindingContext, modelType);
			}
			bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType((Func<object>)null, type);
			return Activator.CreateInstance(type);
		}

		protected Type DetectInstantiationType(ControllerContext controllerContext, ModelBindingContext bindingContext, Type typeToCreate)
		{
			string text = _typeStampOperator.DetectTypeStamp(bindingContext, this);
			if (string.IsNullOrEmpty(text))
			{
				return typeToCreate;
			}
			Type derivedType = DerivedTypeModelBinderCache.GetDerivedType(text);
			if (derivedType != null)
			{
				return derivedType;
			}
			throw new InvalidOperationException($"unable to located identified type '{text}' as a variant of '{typeToCreate.FullName}'");
		}

		public string CreatePropertyName(string prefix, string propertyName)
		{
			return DefaultModelBinder.CreateSubPropertyName(prefix, propertyName);
		}
	}
}
