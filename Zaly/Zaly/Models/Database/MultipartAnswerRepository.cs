using Microsoft.EntityFrameworkCore;

namespace Zaly.Models.Database {
    public sealed class MultipartAnswerRepository : DatabaseRepository<MultipartAnswer> {
        public override void Add(MultipartAnswer entity) {
            DatabaseContext context = new DatabaseContext();
            context.MultipartAnswer.Add(entity);
            context.SaveChanges();
        }

        public override void Delete(int id) {
            var MultipartAnswer = FindById(id);
            if (MultipartAnswer is null) {
                return;
            }
            DatabaseContext context = new DatabaseContext();
            context.MultipartAnswer.Remove(MultipartAnswer);
            context.SaveChanges();
        }

        public override void Delete(MultipartAnswer entity) {
            DatabaseContext context = new DatabaseContext();
            context.MultipartAnswer.Remove(entity);
            context.SaveChanges();
        }

        public override MultipartAnswer? FindById(int id) {
            DatabaseContext context = new DatabaseContext();
            return context.MultipartAnswer.Find(id);
        }

        public override List<MultipartAnswer> GetAll() {
            DatabaseContext context = new DatabaseContext();
            return context.MultipartAnswer.ToList();
        }
        public override void Update(int id, MultipartAnswer entity) {
            DatabaseContext context = new DatabaseContext();
            var dbMultipartAnswer = context.MultipartAnswer.Find(id);
            if (dbMultipartAnswer is null) {
                return;
            }
            dbMultipartAnswer.Answer = entity.Answer;
            dbMultipartAnswer.Correct = entity.Correct;
            dbMultipartAnswer.QuestionId= entity.QuestionId;

            context.SaveChanges();
        }
        public List<MultipartAnswer> FindByFk(int Id) {
            DatabaseContext context = new DatabaseContext();
            //return context.MultipartAnswer.FromSql($"SELECT * FROM MultipartAnswer WHERE QuestionId = {Id}").ToList();
            return context.MultipartAnswer.Where(ma => ma.QuestionId == Id).ToList();
        }
    }
}
