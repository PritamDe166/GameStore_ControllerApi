namespace GameStoreControllerApi.Dto.Contracts.Pagination;

public sealed class PaginationRequest
{
    private const int MaxPageSize = 5;
    public int PageNumber { get; init; } = 1;

    private int _pageSize = 3;
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public int Skip => (PageNumber - 1) * PageSize;
}
