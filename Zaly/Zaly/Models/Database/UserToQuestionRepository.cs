using Microsoft.EntityFrameworkCore;

namespace Zaly.Models.Database {
    public sealed class UserToQuestionRepository : DatabaseRepository<UserToQuestion> {
        public UserToQuestionRepository(DatabaseContext context) {
            _context = context;
        }
        public override void Add(UserToQuestion entity) {
            DatabaseContext context = new DatabaseContext();
            context.UserToQuestion.Add(entity);
            context.SaveChanges();
        }

        public override void Delete(int id) {
            DatabaseContext context = new DatabaseContext();
            var UserToQuestion = FindById(id);
            if (UserToQuestion is null) {
                return;
            }
            context.UserToQuestion.Remove(UserToQuestion);
            context.SaveChanges();
        }

        public override void Delete(UserToQuestion entity) {
            DatabaseContext context = new DatabaseContext();
            context.UserToQuestion.Remove(entity);
            context.SaveChanges();
        }

        public override UserToQuestion? FindById(int id) {
            DatabaseContext context = new DatabaseContext();
            return context.UserToQuestion.Find(id);
        }

        public override List<UserToQuestion> GetAll() {
            DatabaseContext context = new DatabaseContext();
            return context.UserToQuestion.ToList();
        }
        public override void Update(int id, UserToQuestion entity) {
            DatabaseContext context = new DatabaseContext();
            var dbUserToQuestion = context.UserToQuestion.Find(id);
			if (dbUserToQuestion is null) {
				return;
			}
			dbUserToQuestion.Completed = entity.Completed;

			context.SaveChanges();
		}
        public UserToQuestion? FindByFk(int userId, int questionId) {
            DatabaseContext context = new DatabaseContext();
            //var rows = context.UserToQuestion.FromSql($"SELECT * FROM UserToQuestion WHERE UserId = {userId} AND QuestionId = {questionId}").ToList();
            var rows = context.UserToQuestion.Where(utq => utq.UserId == userId && utq.QuestionId == questionId).ToList();
            if (rows.Count != 1) {
                return null;
            }
            return rows[0];
        }
    }
}
