using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LPContribMvc.Pagination
{
	public class CustomPagination<T> : IPagination<T>, IPagination, IEnumerable<T>, IEnumerable
	{
		private readonly IList<T> _dataSource;

		public int PageNumber
		{
			get;
			private set;
		}

		public int PageSize
		{
			get;
			private set;
		}

		public int TotalItems
		{
			get;
			private set;
		}

		public int TotalPages => (int)Math.Ceiling((double)TotalItems / (double)PageSize);

		public int FirstItem => (PageNumber - 1) * PageSize + 1;

		public int LastItem => FirstItem + _dataSource.Count - 1;

		public bool HasPreviousPage => PageNumber > 1;

		public bool HasNextPage => PageNumber < TotalPages;

		public CustomPagination(IEnumerable<T> dataSource, int pageNumber, int pageSize, int totalItems)
		{
			_dataSource = dataSource.ToList();
			PageNumber = pageNumber;
			PageSize = pageSize;
			TotalItems = totalItems;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _dataSource.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
