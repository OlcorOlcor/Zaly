using Microsoft.EntityFrameworkCore;

namespace Zaly.Models.Database {
    public class MultipartAnswerRepository : DatabaseRepository<MultipartAnswer> {
        public override void Add(MultipartAnswer entity) {
            _context.MultipartAnswer.Add(entity);
            _context.SaveChanges();
        }

        public override void Delete(int id) {
            var MultipartAnswer = FindById(id);
            if (MultipartAnswer is null) {
                return;
            }
            _context.MultipartAnswer.Remove(MultipartAnswer);
            _context.SaveChanges();
        }

        public override void Delete(MultipartAnswer entity) {
            _context.MultipartAnswer.Remove(entity);
            _context.SaveChanges();
        }

        public override MultipartAnswer? FindById(int id) {
            return _context.MultipartAnswer.Find(id);
        }

        public override List<MultipartAnswer> GetAll() {
            return _context.MultipartAnswer.ToList();
        }
        public override void Update(int id, MultipartAnswer entity) {
            var dbMultipartAnswer = _context.MultipartAnswer.Find(id);
            if (dbMultipartAnswer is null) {
                return;
            }
            dbMultipartAnswer.Answer = entity.Answer;
            dbMultipartAnswer.Correct = entity.Correct;
            dbMultipartAnswer.QuestionId= entity.QuestionId;

            _context.SaveChanges();
        }
        public List<MultipartAnswer> FindByFk(int Id) {
            return _context.MultipartAnswer.FromSql($"SELECT * FROM MultipartAnswer WHERE QuestionId = {Id}").ToList();
        }
    }
}
