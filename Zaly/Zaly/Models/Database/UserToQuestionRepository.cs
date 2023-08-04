using Microsoft.EntityFrameworkCore;

namespace Zaly.Models.Database {
    public sealed class UserToQuestionRepository : DatabaseRepository<UserToQuestion> {
        public override void Add(UserToQuestion entity) {
            _context.UserToQuestion.Add(entity);
            _context.SaveChanges();
        }

        public override void Delete(int id) {
            var UserToQuestion = FindById(id);
            if (UserToQuestion is null) {
                return;
            }
            _context.UserToQuestion.Remove(UserToQuestion);
            _context.SaveChanges();
        }

        public override void Delete(UserToQuestion entity) {
            _context.UserToQuestion.Remove(entity);
            _context.SaveChanges();
        }

        public override UserToQuestion? FindById(int id) {
            return _context.UserToQuestion.Find(id);
        }

        public override List<UserToQuestion> GetAll() {
            return _context.UserToQuestion.ToList();
        }
        public override void Update(int id, UserToQuestion entity) {
			var dbUserToQuestion = _context.UserToQuestion.Find(id);
			if (dbUserToQuestion is null) {
				return;
			}
			dbUserToQuestion.Completed = entity.Completed;

			_context.SaveChanges();
		}
        public UserToQuestion? FindByFk(int userId, int questionId) {
            var rows = _context.UserToQuestion.FromSql($"SELECT * FROM UserToQuestion WHERE UserId = {userId} AND QuestionId = {questionId}").ToList();
            if (rows.Count != 1) {
                return null;
            }
            return rows[0];
        }
    }
}
