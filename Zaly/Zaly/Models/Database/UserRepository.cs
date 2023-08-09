using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace Zaly.Models.Database
{
    public sealed class UserRepository : DatabaseRepository<User>
    {
        public override void Add(User entity)
        {
            DatabaseContext context = new DatabaseContext();
            context.User.Add(entity);
            context.SaveChanges();
        }

        public override void Delete(int id)
        {
            var user = FindById(id);
            if (user is null)
            {
                return;
            }
            Delete(user);
        }

        public override void Delete(User entity)
        {
            DatabaseContext context = new DatabaseContext();
            //var links = context.UserToQuestion.FromSql($"SELECT * FROM UserToQuestion WHERE UserId = {entity.Id}").ToList();
            var links = context.UserToQuestion.Where(utq => utq.UserId == entity.Id).ToList();
            if (links.Count != 0) {
                context.UserToQuestion.RemoveRange(links);
                context.SaveChanges();
            }
            context.User.Remove(entity);
            context.SaveChanges();
        }

        public override User? FindById(int id)
        {
            DatabaseContext context = new DatabaseContext();
            return context.User.Find(id);
        }

        public override List<User> GetAll()
        {
            DatabaseContext context = new DatabaseContext();
            return context.User.ToList();
        }
        public override void Update(int id, User entity)
        {
            DatabaseContext context = new DatabaseContext();
            var dbUser = context.User.Find(id);
            if (dbUser is null)
            {
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

            context.SaveChanges();
        }

        public User? Login(string Login, string Password)
        {
            DatabaseContext context = new DatabaseContext();
            //var users = context.User.FromSql($"Select * from User where Login = {Login}").ToList();
            var users = context.User.Where(u => u.Login == Login).ToList();
            if (users.Count() != 1)
            {
                return null;
            }
            var user = users[0];
            Hasher pm = new Hasher();
            if (pm.VerifyPassword(Password, user.Password.Substring(0, user.Password.Length / 2), user.Password.Substring(user.Password.Length / 2, user.Password.Length / 2)))
            {
                return user;
            }
            return null;
        }
    }
}
