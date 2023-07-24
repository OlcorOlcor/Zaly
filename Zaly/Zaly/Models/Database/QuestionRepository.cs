﻿using Microsoft.EntityFrameworkCore;

namespace Zaly.Models.Database {
    public class QuestionRepository : DatabaseRepository<Question> {
        public override void Add(Question entity) {
            _context.Question.Add(entity);
            _context.SaveChanges();
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
    }
}