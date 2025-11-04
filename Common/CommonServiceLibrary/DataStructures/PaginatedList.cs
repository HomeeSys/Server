namespace CommonServiceLibrary.DataStructures;

public class PaginatedList<T>
{
    public PaginatedList(List<T> items, int page, int pageSize, int totalCount, int absoluteCount)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        AbsoluteCount = absoluteCount;
    }

    public List<T> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int TotalCount { get; }
    public int AbsoluteCount { get; }
    public bool HasNextPage => Page * PageSize < TotalCount;
    public bool HasPreviousPage => Page > 1;
    public static async Task<PaginatedList<T>> Create(IQueryable<T> query, int page, int pageSize, int absoluteCount)
    {
        var totalCount = query.Count();
        var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return new(items, page, pageSize, totalCount, absoluteCount);
    }
}