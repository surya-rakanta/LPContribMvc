using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LPContribMvc.Pagination
{
	public class LazyPagination<T> : IPagination<T>, IPagination, IEnumerable<T>, IEnumerable
	{
		public const int DefaultPageSize = 20;

		private IList<T> results;

		private int totalItems;

		public int PageSize
		{
			get;
			private set;
		}

		public IQueryable<T> Query
		{
			get;
			protected set;
		}

		public int PageNumber
		{
			get;
			private set;
		}

		public int TotalItems
		{
			get
			{
				TryExecuteQuery();
				return totalItems;
			}
		}

		public int TotalPages => (int)Math.Ceiling((double)TotalItems / (double)PageSize);

		public int FirstItem
		{
			get
			{
				TryExecuteQuery();
				return (PageNumber - 1) * PageSize + 1;
			}
		}

		public int LastItem => FirstItem + results.Count - 1;

		public bool HasPreviousPage => PageNumber > 1;

		public bool HasNextPage => PageNumber < TotalPages;

		public LazyPagination(IQueryable<T> query, int pageNumber, int pageSize)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
			Query = query;
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			TryExecuteQuery();
			foreach (T result in results)
			{
				yield return result;
			}
		}

		protected void TryExecuteQuery()
		{
			if (results == null)
			{
				totalItems = Query.Count();
				results = ExecuteQuery();
			}
		}

		protected virtual IList<T> ExecuteQuery()
		{
			int count = (PageNumber - 1) * PageSize;
			return Query.Skip(count).Take(PageSize).ToList();
		}

		public IEnumerator GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}
}
