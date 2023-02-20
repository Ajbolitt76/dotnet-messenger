using System.Linq.Expressions;
using Messenger.Core.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static async Task<T> FirstOrNotFound<T>(
        this IQueryable<T> queryable,
        CancellationToken cancellationToken = default)
    {
        return await queryable.FirstOrDefaultAsync(cancellationToken) 
            ?? throw new NotFoundException<T>();
    }
    
    
    public static async Task<T> FirstOrNotFound<T>(
        this IQueryable<T> queryable,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
    {
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken) 
            ?? throw new NotFoundException<T>();
    }
}
