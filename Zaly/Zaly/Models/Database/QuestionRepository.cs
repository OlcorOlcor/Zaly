using Microsoft.EntityFrameworkCore;

namespace Zaly.Models.Database {
    public class QuestionRepository : DatabaseRepository<Question> {
        public override void Add(Question entity) {
            _context.Question.Add(entity);
            _context.SaveChanges();
            
            Hasher hasher = new Hasher();
            entity.Code = hasher.HashCode(entity.Id);
            Update(entity.Id, entity);
        }

        public override void Delete(int id) {
            var Question = FindById(id);
            if (Question is null) {
                return;
            }
            _context.Question.Remove(Question);
            _context.SaveChanges(); 
        }

        public List<MultipartAnswer> GetMultipartAnswersForQuestion(int QuestionId) {
            return _context.MultipartAnswer.FromSql($"SELECT * FROM MultipartAnswer ma WHERE ma.QuestionId = {QuestionId}").ToList();
        }

        public override void Delete(Question entity) {
            _context.Question.Remove(entity);
            _context.SaveChanges();
        }

        public override Question? FindById(int id) {
            return _context.Question.Find(id);
        }

        public override List<Question> GetAll() {
            return _context.Question.ToList();
        }
        public override void Update(int id, Question entity) {
            var dbQuestion = _context.Question.Find(id);
            if (dbQuestion is null) {
                return;
            }
            dbQuestion.Code = entity.Code;
            dbQuestion.Name = entity.Name;
            dbQuestion.Text = entity.Text;
            dbQuestion.Multipart = entity.Multipart;
            dbQuestion.Answer = entity.Answer;
            dbQuestion.Points = entity.Points;
            dbQuestion.Img = entity.Img;

            _context.SaveChanges();
        }
        public Question? FindByCode(string code) {
            var codeList = _context.Question.FromSql($"SELECT * FROM Question WHERE code = {code}").ToList();
            if (codeList.Count != 1) {
                return null;
            }
            return codeList[0];
        }
        public List<Question> GetQuestionsForGivenUser(int userId) {
            return _context.Question.FromSql($"SELECT q.* FROM Question q INNER JOIN UserToQuestion uq on q.Id = uq.QuestionId INNER JOIN User u on u.Id = uq.UserId WHERE u.Id = {userId}").ToList();
        }
    }
}