using LPContribMvc.UI.InputBuilder.Conventions;
using System.Collections.Generic;

namespace LPContribMvc.UI.InputBuilder
{
	public class DefaultPropertyConventionsFactory : List<IPropertyViewModelFactory>
	{
		public DefaultPropertyConventionsFactory()
		{
			Add(new ArrayPropertyConvention());
			Add(new GuidPropertyConvention());
			Add(new EnumPropertyConvention());
			Add(new DateTimePropertyConvention());
			Add(new DefaultPropertyConvention());
		}
	}
}
