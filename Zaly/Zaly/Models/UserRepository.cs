namespace Zaly.Models {
	public sealed class UserRepository : DatabaseRepository<User> {
		public override void Add(User entity) {
			_context.User.Add(entity);
			_context.SaveChanges();
		}

		public override void Delete(int id) {
			var user = FindById(id);
			if (user is null) { 
				return;
			}
			_context.User.Remove(user);
			_context.SaveChanges();
		}

		public override void Delete(User entity) {
			_context.User.Remove(entity); 
			_context.SaveChanges();
		}

		public override User? FindById(int id) {
			return _context.User.Find(id);
		}

		public override List<User> GetAll() {
			return _context.User.ToList();
		}
		public override void Update(int id, User entity) {
			var dbUser = _context.User.Find(id);
			if (dbUser is null) {
				return;
			}
			dbUser.Name = entity.Name;
			dbUser.Surname = entity.Surname;
			dbUser.Points = entity.Points;
			dbUser.Nickname = entity.Nickname;
			dbUser.Password = entity.Password;
			dbUser.TeamId = entity.TeamId;

			_context.SaveChanges();
		}
	}
}
