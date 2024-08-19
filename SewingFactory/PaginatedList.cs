using System;


public class PaginatedList<T>
{
	public IEnumerable<T> Items { get; set; }
	public int Page { get; set; }
	public int PageSize { get; set; }
	public PaginatedList(IEnumerable<T> items, int page, int pageSize)
	{
		Items = items;
		Page = page;
		PageSize = pageSize;
	}



}
