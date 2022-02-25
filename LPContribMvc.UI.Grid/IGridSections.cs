namespace LPContribMvc.UI.Grid
{
	public interface IGridSections<T> where T : class
	{
		GridRow<T> Row
		{
			get;
		}

		GridRow<T> HeaderRow
		{
			get;
		}
	}
}
