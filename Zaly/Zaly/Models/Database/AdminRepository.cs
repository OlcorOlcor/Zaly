using Microsoft.EntityFrameworkCore;

namespace Zaly.Models.Database
{
    public sealed class AdminRepository : DatabaseRepository<Admin>
    {
        public AdminRepository(DatabaseContext context) {
            _context = context;
        }
        public override void Add(Admin entity)
        {   
            DatabaseContext context = new DatabaseContext();
            context.Admin.Add(entity);
            context.SaveChanges();
        }

        public override void Delete(int id)
        {
            DatabaseContext context = new DatabaseContext();
            var admin = FindById(id);
            if (admin is null)
            {
                return;
            }
            context.Admin.Remove(admin);
            context.SaveChanges();
        }

        public override void Delete(Admin entity)
        {
            DatabaseContext context = new DatabaseContext();
            context.Admin.Remove(entity);
            context.SaveChanges();
        }

        public override Admin? FindById(int id)
        {
            DatabaseContext context = new DatabaseContext();
            return context.Admin.Find(id);
        }

        public override List<Admin> GetAll()
        {
            DatabaseContext context = new DatabaseContext();
            return context.Admin.ToList();
        }
        public override void Update(int id, Admin entity)
        {
            DatabaseContext context = new DatabaseContext();
            var dbAdmin = context.Admin.Find(id);
            if (dbAdmin is null)
            {
                return;
            }
            dbAdmin.Name = entity.Name;
            dbAdmin.Surname = entity.Surname;
            dbAdmin.Password = entity.Password;

            context.SaveChanges();
        }

        public Admin? Login(string Login, string Password)
        {
            DatabaseContext context = new DatabaseContext();
            //var admins = context.Admin.FromSql($"Select * from Admin where Login = {Login}").ToList();
            var admins = context.Admin.Where(a => a.Login == Login).ToList();
            if (admins.Count() != 1)
            {
                return null;
            }
            var admin = admins[0];
            Hasher pm = new Hasher();
            if (pm.VerifyPassword(Password, admin.Password.Substring(0, admin.Password.Length / 2), admin.Password.Substring(admin.Password.Length / 2, admin.Password.Length / 2)))
            {
                return admin;
            }
            return null;
        }
    }
}
