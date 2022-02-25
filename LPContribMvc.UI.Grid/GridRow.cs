using System;
using System.Collections.Generic;

namespace LPContribMvc.UI.Grid
{
	public class GridRow<T>
	{
		private Func<GridRowViewData<T>, IDictionary<string, object>> _attributes = (GridRowViewData<T> x) => new Dictionary<string, object>();

		private Func<GridRowViewData<T>, RenderingContext, bool> _startSectionRenderer = (GridRowViewData<T> x, RenderingContext y) => false;

		private Func<GridRowViewData<T>, RenderingContext, bool> _endSectionRenderer = (GridRowViewData<T> x, RenderingContext y) => false;

		public Func<GridRowViewData<T>, RenderingContext, bool> StartSectionRenderer
		{
			get
			{
				return _startSectionRenderer;
			}
			set
			{
				_startSectionRenderer = value;
			}
		}

		public Func<GridRowViewData<T>, RenderingContext, bool> EndSectionRenderer
		{
			get
			{
				return _endSectionRenderer;
			}
			set
			{
				_endSectionRenderer = value;
			}
		}

		public Func<GridRowViewData<T>, IDictionary<string, object>> Attributes
		{
			get
			{
				return _attributes;
			}
			set
			{
				_attributes = value;
			}
		}
	}
}
