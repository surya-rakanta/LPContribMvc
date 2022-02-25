namespace LPContribMvc.UI.Grid
{
	public class GridRowViewData<T>
	{
		public T Item
		{
			get;
			private set;
		}

		public bool IsAlternate
		{
			get;
			private set;
		}

		public GridRowViewData(T item, bool isAlternate)
		{
			Item = item;
			IsAlternate = isAlternate;
		}
	}
}
