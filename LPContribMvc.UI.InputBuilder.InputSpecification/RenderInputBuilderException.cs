using System;

namespace LPContribMvc.UI.InputBuilder.InputSpecification
{
	public class RenderInputBuilderException : Exception
	{
		public RenderInputBuilderException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
