namespace Zaly.Models.Database
{
    public interface IRepository<T>
    {
        public List<T> GetAll();
        public T? FindById(int id);
        public void Add(T entity);
        public void Update(int id, T entity);
        public void Delete(int id);
        public void Delete(T entity);
    }
}
