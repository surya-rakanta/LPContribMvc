using System.Web;

namespace LPContribMvc.UI.Grid.Syntax
{
	public interface IGrid<T> : IGridWithOptions<T>, IHtmlString where T : class
	{
		IGrid<T> WithModel(IGridModel<T> model);
	}
}
