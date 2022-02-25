using LPContribMvc.UI.InputBuilder.Views;
using System;
using System.Web.Mvc;

namespace LPContribMvc.UI.InputBuilder.InputSpecification
{
	public class InputPropertySpecification : IInputSpecification<PropertyViewModel>, IInputSpecification<TypeViewModel>
	{
		public Func<HtmlHelper, PropertyViewModel, string> Render = delegate(HtmlHelper helper, PropertyViewModel model)
		{
			helper.RenderPartial(model.PartialName, model, model.Layout);
			return "";
		};

		public HtmlHelper HtmlHelper
		{
			get;
			set;
		}

		public PropertyViewModel Model
		{
			get;
			set;
		}

		TypeViewModel IInputSpecification<TypeViewModel>.Model => Model;

		public override string ToString()
		{
			return Render(HtmlHelper, Model);
		}
	}
}
