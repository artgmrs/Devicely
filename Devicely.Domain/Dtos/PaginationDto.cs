namespace Devicely.Domain.Dtos;

public class PaginationDto
{
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
}
