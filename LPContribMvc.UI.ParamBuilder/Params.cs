using LPContribMvc.UI.ReturnUrl;
using System.Collections.Generic;

namespace LPContribMvc.UI.ParamBuilder
{
	public class Params
	{
		public static ParamBuilder With => new ParamBuilder(new ReturnUrlManager());

		public static IDictionary<string, object> Empty => new Dictionary<string, object>();
	}
}
