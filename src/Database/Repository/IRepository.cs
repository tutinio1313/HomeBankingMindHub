using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace HomeBankingMindHub.Database.Repository;

public interface IRepository<T>
{
    IQueryable<T> FindAll();
    #pragma warning disable
    IQueryable<T> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
    #pragma warning restore
    IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression);

    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    int SaveChanges();
}