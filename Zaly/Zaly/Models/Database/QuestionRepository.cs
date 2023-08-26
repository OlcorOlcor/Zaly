using Microsoft.EntityFrameworkCore;

namespace Zaly.Models.Database {
    public sealed class QuestionRepository : DatabaseRepository<Question> {
        private Hasher _hasher;
        public QuestionRepository(DatabaseContext context, Hasher hasher) {
            _context = context;
            _hasher = hasher;
        }
        public override void Add(Question entity) {
            _context.Question.Add(entity);
            _context.SaveChanges();
            
            entity.Code = _hasher.HashCode(entity.Id);
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
            dbQuestion.Name = entity.Name;
            dbQuestion.Text = entity.Text;
            dbQuestion.Multipart = entity.Multipart;
            dbQuestion.Answer = entity.Answer;
            dbQuestion.Points = entity.Points;
            dbQuestion.Img = entity.Img;

            _context.SaveChanges();
        }
        public Question? FindByCode(string code) {
            var codeList = _context.Question.Where(q => q.Code == code).ToList();
            if (codeList.Count != 1) {
                return null;
            }
            return codeList[0];
        }
        public List<Question> GetQuestionsForGivenUser(int userId) {
            return (from q in _context.Question
                    join utq in _context.UserToQuestion on q.Id equals utq.QuestionId
                    join u in _context.User on utq.UserId equals u.Id
                    where u.Id == userId
                    select q).ToList();
        }
    }
}