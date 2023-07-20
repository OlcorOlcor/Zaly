namespace Zaly.Models {
	public sealed class UserRepository : DatabaseRepository<User> {
		public override void Add(User entity) {
			_context.User.Add(entity);
			_context.SaveChanges();
		}

		public override void Delete(int id) {
			var user = FindById(id);
			var a = _context.User.Remove(user);
			_context.SaveChanges();
		}

		public override void Delete(User entity) {
			throw new NotImplementedException();
		}

		public override User FindById(int id) {
			throw new NotImplementedException();
		}

		public override List<User> GetAll() {
			throw new NotImplementedException();
		}

		public override List<User> GetWhere(string condition) {
			throw new NotImplementedException();
		}

		public override void Update(int id, User entity) {
			throw new NotImplementedException();
		}
	}
}
