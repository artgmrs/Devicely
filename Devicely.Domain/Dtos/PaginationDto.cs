using Devicely.Domain.Constants;

namespace Devicely.Domain.Dtos;

public class PaginationDto
{
    public int PageSize { get; set; } = PaginationConstants.DefaultPageSize;
    public int PageNumber { get; set; } = PaginationConstants.DefaultPageNumber;
}