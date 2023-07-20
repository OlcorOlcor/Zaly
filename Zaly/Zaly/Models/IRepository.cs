namespace Zaly.Models {
	public interface IRepository<T> {
		public List<T> GetAll();
		public List<T> GetWhere(string condition);
		public T FindById(int id);
		public void Add(T entity);
		public void Update(int id, T entity);
		public void Delete(int id);
		public void Delete(T entity);
	}
}
