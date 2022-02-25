using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace LPContribMvc.UI.Grid
{
	public interface IGridRenderer<T> where T : class
	{
		void Render(IGridModel<T> gridModel, IEnumerable<T> dataSource, TextWriter output, ViewContext viewContext);
	}
}
