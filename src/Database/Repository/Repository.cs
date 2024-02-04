using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HomeBankingMindHub.Database.Repository;

public abstract class Repository<T> : IRepository<T> where T : class
{
    protected HomeBankingContext context { get; set; }

    public Repository(HomeBankingContext context) { this.context = context; }

    public IQueryable<T> FindAll() => context.Set<T>();

    public IQueryable<T> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
    {
        IQueryable<T> queryable = context.Set<T>();

        if (includes is not null)
        {
            queryable = includes(queryable);
        }
        return queryable;
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => context.Set<T>().Where(expression);

    public void Create(T entity) => context.Set<T>().Add(entity);

    public void Update(T entity) => context.Set<T>().Update(entity);
    public void Delete(T entity) => context.Set<T>().Remove(entity);

    public void SaveChanges() { context.SaveChanges(); }
}