namespace Zaly.Models.Database {
    public sealed class UserRepository : DatabaseRepository<User> {
        public UserRepository(DatabaseContext context) {
            _context = context;
        }
        public override void Add(User entity) {
            _context.User.Add(entity);
            _context.SaveChanges();
        }

        public override void Delete(int id) {
            var user = FindById(id);
            if (user is null) {
                return;
            }
            Delete(user);
        }

        public override void Delete(User entity) {
            var links = _context.UserToQuestion.Where(utq => utq.UserId == entity.Id).ToList();
            if (links.Count != 0) {
                _context.UserToQuestion.RemoveRange(links);
                _context.SaveChanges();
            }
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
            dbUser.Login = entity.Name.ToLower() + entity.Surname.ToLower();
            if (entity.Password != null && entity.Password != "") {
                dbUser.Password = entity.Password;
            }
            dbUser.TeamId = entity.TeamId;

            _context.SaveChanges();
        }

        public User? Login(string Login, string Password) {
            var users = _context.User.Where(u => u.Login == Login).ToList();
            if (users.Count() != 1) {
                return null;
            }
            var user = users[0];
            Hasher pm = new Hasher();
            if (pm.VerifyPassword(Password, user.Password.Substring(0, user.Password.Length / 2), user.Password.Substring(user.Password.Length / 2, user.Password.Length / 2))) {
                return user;
            }
            return null;
        }
    }
}
