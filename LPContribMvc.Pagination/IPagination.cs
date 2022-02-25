using System.Collections;
using System.Collections.Generic;

namespace LPContribMvc.Pagination
{
	public interface IPagination : IEnumerable
	{
		int PageNumber
		{
			get;
		}

		int PageSize
		{
			get;
		}

		int TotalItems
		{
			get;
		}

		int TotalPages
		{
			get;
		}

		int FirstItem
		{
			get;
		}

		int LastItem
		{
			get;
		}

		bool HasPreviousPage
		{
			get;
		}

		bool HasNextPage
		{
			get;
		}
	}
	public interface IPagination<T> : IPagination, IEnumerable<T>, IEnumerable
	{
	}
}
