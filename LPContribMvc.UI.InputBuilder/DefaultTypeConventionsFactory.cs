using LPContribMvc.UI.InputBuilder.InputSpecification;
using System.Collections.Generic;

namespace LPContribMvc.UI.InputBuilder
{
	public class DefaultTypeConventionsFactory : List<ITypeViewModelFactory>
	{
		public DefaultTypeConventionsFactory()
		{
			Add(new DefaultTypeViewModelFactoryConvention());
		}
	}
}
