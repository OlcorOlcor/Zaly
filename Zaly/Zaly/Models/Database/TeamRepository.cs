using Microsoft.EntityFrameworkCore;

namespace Zaly.Models.Database
{
    public sealed class TeamRepository : DatabaseRepository<Team>
    {
        public TeamRepository(DatabaseContext context) {
            _context = context;
        }
        public override void Add(Team entity)
        {
            DatabaseContext context = new DatabaseContext();
            context.Team.Add(entity);
            context.SaveChanges();
        }
        public List<User> GetTeamUsers(Team entity)
        {
            DatabaseContext context = new DatabaseContext();
            //return context.User.FromSql($"SELECT * FROM USER u WHERE u.TeamId = {entity.Id}").ToList();
            return context.User.Where(u => u.TeamId == entity.Id).ToList();
        }
        public override void Delete(int id)
        {
            DatabaseContext context = new DatabaseContext();
            var Team = FindById(id);
            if (Team is null)
            {
                return;
            }
            context.Team.Remove(Team);
            context.SaveChanges();
        }

        public override void Delete(Team entity)
        {
            DatabaseContext context = new DatabaseContext();
            context.Team.Remove(entity);
            context.SaveChanges();
        }

        public override Team? FindById(int id)
        {
            DatabaseContext context = new DatabaseContext();
            return context.Team.Find(id);
        }

        public override List<Team> GetAll()
        {
            DatabaseContext context = new DatabaseContext();
            return context.Team.ToList();
        }
        public override void Update(int id, Team entity)
        {
            DatabaseContext context = new DatabaseContext();
            var dbTeam = context.Team.Find(id);
            if (dbTeam is null)
            {
                return;
            }
            dbTeam.Name = entity.Name;

            context.SaveChanges();
        }
        public int GetTeamPoints(int teamId) {
            DatabaseContext context = new DatabaseContext();
            //return context.User.FromSql($"SELECT u.* FROM User u INNER JOIN Team t on t.Id = u.TeamId WHERE t.Id = {teamId}").Sum(x => x.Points);
            return (from u in context.User
                    join t in context.Team on u.TeamId equals t.Id
                    where t.Id == teamId
                    select u).Sum(u => u.Points);
        }
    }
}
