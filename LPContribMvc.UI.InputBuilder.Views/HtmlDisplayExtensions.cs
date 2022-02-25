using LPContribMvc.UI.InputBuilder.InputSpecification;
using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace LPContribMvc.UI.InputBuilder.Views
{
	public static class HtmlDisplayExtensions
	{
		public static IInputSpecification<PropertyViewModel> Display<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, object>> expression) where TModel : class
		{
			IInputSpecification<PropertyViewModel> inputSpecification = helper.Input(expression);
			inputSpecification.Model.Layout = "Display";
			inputSpecification.Model.PartialName = "DisplayParagraph";
			return inputSpecification;
		}

		public static IInputSpecification<PropertyViewModel> Label<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, object>> expression) where TModel : class
		{
			IInputSpecification<PropertyViewModel> inputSpecification = helper.Input(expression);
			inputSpecification.Model.Layout = "Display";
			inputSpecification.Model.PartialName = "DisplayLabel";
			return inputSpecification;
		}
	}
}
