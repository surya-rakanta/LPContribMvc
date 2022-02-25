using LPContribMvc.UI.InputBuilder.Conventions;
using LPContribMvc.UI.InputBuilder.Helpers;
using LPContribMvc.UI.InputBuilder.Views;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace LPContribMvc.UI.InputBuilder.InputSpecification
{
	public class InputTypeSpecification<T> : IInputSpecification<TypeViewModel> where T : class
	{
		public HtmlHelper<T> HtmlHelper
		{
			get;
			set;
		}

		public string Controller
		{
			get;
			set;
		}

		public string Action
		{
			get;
			set;
		}

		public TypeViewModel Model
		{
			get;
			set;
		}

		public override string ToString()
		{
			ViewModelFactory<T> viewModelFactory = new ViewModelFactory<T>(HtmlHelper, InputBuilder.Conventions.ToArray(), new DefaultNameConvention(), InputBuilder.TypeConventions.ToArray());
			List<PropertyViewModel> list = new List<PropertyViewModel>();
			PropertyInfo[] array = Model.Type.GetProperties().ReOrderProperties();
			foreach (PropertyInfo propertyInfo in array)
			{
				list.Add(viewModelFactory.Create(propertyInfo, Model.Name));
			}
			RenderPartialExtensions.RenderPartial((HtmlHelper)(object)HtmlHelper, Model.PartialName, (object)list.ToArray());
			return string.Empty;
		}

		protected virtual void RenderPartial(PropertyViewModel model)
		{
			((HtmlHelper)(object)HtmlHelper).RenderPartial(model.PartialName, model, model.Layout);
		}
	}
}
