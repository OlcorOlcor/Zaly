using Microsoft.EntityFrameworkCore;

namespace Zaly.Models.Database {
    public sealed class QuestionRepository : DatabaseRepository<Question> {
        public QuestionRepository(DatabaseContext context) {
            _context = context;
        }
        public override void Add(Question entity) {
            DatabaseContext context = new DatabaseContext();
            context.Question.Add(entity);
            context.SaveChanges();
            
            Hasher hasher = new Hasher();
            entity.Code = hasher.HashCode(entity.Id);
            Update(entity.Id, entity);
        }

        public override void Delete(int id) {
            DatabaseContext context = new DatabaseContext();
            var Question = FindById(id);
            if (Question is null) {
                return;
            }
            context.Question.Remove(Question);
            context.SaveChanges(); 
        }

        public List<MultipartAnswer> GetMultipartAnswersForQuestion(int QuestionId) {
            DatabaseContext context = new DatabaseContext();
            return context.MultipartAnswer.FromSql($"SELECT * FROM MultipartAnswer ma WHERE ma.QuestionId = {QuestionId}").ToList();
        }

        public override void Delete(Question entity) {
            DatabaseContext context = new DatabaseContext();
            context.Question.Remove(entity);
            context.SaveChanges();
        }

        public override Question? FindById(int id) {
            DatabaseContext context = new DatabaseContext();
            return context.Question.Find(id);
        }

        public override List<Question> GetAll() {
            DatabaseContext context = new DatabaseContext();
            return context.Question.ToList();
        }
        public override void Update(int id, Question entity) {
            DatabaseContext context = new DatabaseContext();
            var dbQuestion = context.Question.Find(id);
            if (dbQuestion is null) {
                return;
            }
            dbQuestion.Name = entity.Name;
            dbQuestion.Text = entity.Text;
            dbQuestion.Multipart = entity.Multipart;
            dbQuestion.Answer = entity.Answer;
            dbQuestion.Points = entity.Points;
            dbQuestion.Img = entity.Img;

            context.SaveChanges();
        }
        public Question? FindByCode(string code) {
            DatabaseContext context = new DatabaseContext();
            //var codeList = context.Question.FromSql($"SELECT * FROM Question WHERE code = {code}").ToList();
            var codeList = context.Question.Where(q => q.Code == code).ToList();
            if (codeList.Count != 1) {
                return null;
            }
            return codeList[0];
        }
        public List<Question> GetQuestionsForGivenUser(int userId) {
            DatabaseContext context = new DatabaseContext();
            //return context.Question.FromSql($"SELECT DISTINCT q.* FROM Question q INNER JOIN UserToQuestion uq on q.Id = uq.QuestionId INNER JOIN User u on u.Id = uq.UserId WHERE u.Id = {userId}").ToList();
            return (from q in context.Question
                    join utq in context.UserToQuestion on q.Id equals utq.QuestionId
                    join u in context.User on utq.UserId equals u.Id
                    where u.Id == userId
                    select q).ToList();
        }
    }
}