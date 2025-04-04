namespace pos_system.Repository
{
    public interface ICrudRepo<T> where T : class
    {
        Task<List<T>> GetAll();
        Task<T> GetById(string id);
        Task Add(T entity);
        Task AddRange(List<T> entities);
        Task Update(T entity);
        Task Delete(string id);
    }
}
