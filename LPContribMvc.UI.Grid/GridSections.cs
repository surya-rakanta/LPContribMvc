namespace LPContribMvc.UI.Grid
{
	public class GridSections<T> : IGridSections<T> where T : class
	{
		private GridRow<T> _row = new GridRow<T>();

		private GridRow<T> _headerRow = new GridRow<T>();

		GridRow<T> IGridSections<T>.Row => _row;

		GridRow<T> IGridSections<T>.HeaderRow => _headerRow;
	}
}
