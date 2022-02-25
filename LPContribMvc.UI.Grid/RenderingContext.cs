using System.IO;
using System.Web.Mvc;

namespace LPContribMvc.UI.Grid
{
	public class RenderingContext
	{
		public TextWriter Writer
		{
			get;
			private set;
		}

		public ViewContext ViewContext
		{
			get;
			private set;
		}

		public ViewEngineCollection ViewEngines
		{
			get;
			private set;
		}

		public RenderingContext(TextWriter writer, ViewContext viewContext, ViewEngineCollection viewEngines)
		{
			Writer = writer;
			ViewContext = viewContext;
			ViewEngines = viewEngines;
		}
	}
}
