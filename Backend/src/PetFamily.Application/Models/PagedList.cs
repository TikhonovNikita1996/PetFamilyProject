namespace PetFamily.Application.Models;

public class PagedList<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalCount { get; init; }
    public bool HasNextPage => PageNumber * PageNumber < TotalCount;
    public bool HasPreviousPage => PageNumber > 1;
}