using Microsoft.EntityFrameworkCore;
using API.Entities;

namespace API.Extensions;

public static class PaginationExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedResultAsync<T>(
        this IQueryable<T> query, int pageNumber, int pageSize)
    {
        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResult<T>
        {
            Items = items,
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }
}