namespace Zaly.Models {
	public abstract class DatabaseRepository<T> : IRepository<T> {
        protected DatabaseContext _context { get; set; } = new DatabaseContext();

		public abstract void Add(T entity);

		public abstract void Delete(int id);

		public abstract void Delete(T entity);

		public abstract T? FindById(int id);

		public abstract List<T> GetAll();

		public abstract void Update(int id, T entity);
	}
}
