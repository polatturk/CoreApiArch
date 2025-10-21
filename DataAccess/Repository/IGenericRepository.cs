namespace DataAccess.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {

        #region CRUD

        void Create(TEntity entity);
        IQueryable<TEntity> GetAll();
        void Update(TEntity entity);
        void Delete(TEntity entity);
        #endregion

        Task<TEntity> GetByIdAsync(int id);
        void DeleteRange(List<TEntity> entities);
    }
}