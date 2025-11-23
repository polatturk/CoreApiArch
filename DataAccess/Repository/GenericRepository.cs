using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        protected readonly APIContext _context;
        private readonly DbSet<TEntity> _dbset;

        public GenericRepository(APIContext context)
        {
            _context = context;
            _dbset = context.Set<TEntity>();
        }
        public void Create(TEntity entity)
        {
            _dbset.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _dbset.Remove(entity);
            _context.SaveChanges();
        }

        public void DeleteRange(List<TEntity> entities)
        {
            _dbset.RemoveRange(entities);
            _context.SaveChanges();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _dbset.AsNoTracking();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
           return await _dbset.FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            _dbset.Update(entity);
            _context.SaveChanges();
        }

        public IQueryable<TEntity> Queryable()
        {
            return _dbset.AsQueryable();
        }
    }
}
