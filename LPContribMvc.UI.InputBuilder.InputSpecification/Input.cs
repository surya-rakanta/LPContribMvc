using LPContribMvc.UI.InputBuilder.Conventions;
using LPContribMvc.UI.InputBuilder.Views;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace LPContribMvc.UI.InputBuilder.InputSpecification
{
	public class Input<T> where T : class
	{
		private readonly HtmlHelper<T> _htmlHelper;

		public Input(HtmlHelper<T> htmlHelper)
		{
			_htmlHelper = htmlHelper;
		}

		public IInputSpecification<PropertyViewModel> RenderInput(Expression<Func<T, object>> expression)
		{
			InputPropertySpecification inputPropertySpecification = new InputPropertySpecification();
			inputPropertySpecification.Model = new ViewModelFactory<T>(_htmlHelper, InputBuilder.Conventions.ToArray(), new DefaultNameConvention(), InputBuilder.TypeConventions.ToArray()).Create(expression);
			inputPropertySpecification.HtmlHelper = (HtmlHelper)(object)_htmlHelper;
			return inputPropertySpecification;
		}

		public IInputSpecification<TypeViewModel> RenderForm(string controller, string action)
		{
			InputTypeSpecification<T> inputTypeSpecification = new InputTypeSpecification<T>();
			inputTypeSpecification.HtmlHelper = _htmlHelper;
			inputTypeSpecification.Controller = controller;
			inputTypeSpecification.Action = action;
			return inputTypeSpecification;
		}

		public IInputSpecification<TypeViewModel> RenderForm()
		{
			InputTypeSpecification<T> inputTypeSpecification = new InputTypeSpecification<T>();
			inputTypeSpecification.Model = new ViewModelFactory<T>(_htmlHelper, InputBuilder.Conventions.ToArray(), new DefaultNameConvention(), InputBuilder.TypeConventions.ToArray()).Create();
			inputTypeSpecification.HtmlHelper = _htmlHelper;
			return inputTypeSpecification;
		}
	}
}
