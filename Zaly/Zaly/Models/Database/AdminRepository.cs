using Microsoft.EntityFrameworkCore;

namespace Zaly.Models.Database
{
    public sealed class AdminRepository : DatabaseRepository<Admin>
    {
        private Hasher _hasher;
        public AdminRepository(DatabaseContext context, Hasher hasher) {
            _context = context;
            _hasher = hasher;
        }
        public override void Add(Admin entity)
        {   
            _context.Admin.Add(entity);
            _context.SaveChanges();
        }

        public override void Delete(int id)
        {
            var admin = FindById(id);
            if (admin is null)
            {
                return;
            }
            _context.Admin.Remove(admin);
            _context.SaveChanges();
        }

        public override void Delete(Admin entity)
        {
            _context.Admin.Remove(entity);
            _context.SaveChanges();
        }

        public override Admin? FindById(int id)
        {
            return _context.Admin.Find(id);
        }

        public override List<Admin> GetAll()
        {
            return _context.Admin.ToList();
        }
        public override void Update(int id, Admin entity)
        {
            var dbAdmin = _context.Admin.Find(id);
            if (dbAdmin is null)
            {
                return;
            }
            dbAdmin.Name = entity.Name;
            dbAdmin.Surname = entity.Surname;
            dbAdmin.Password = entity.Password;

            _context.SaveChanges();
        }

        public Admin? Login(string Login, string Password)
        {
            var admins = _context.Admin.Where(a => a.Login == Login).ToList();
            if (admins.Count() != 1)
            {
                return null;
            }
            var admin = admins[0];
            if (_hasher.VerifyPassword(Password, admin.Password.Substring(0, admin.Password.Length / 2), admin.Password.Substring(admin.Password.Length / 2, admin.Password.Length / 2)))
            {
                return admin;
            }
            return null;
        }
    }
}
