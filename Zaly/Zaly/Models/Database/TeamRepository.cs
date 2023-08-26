namespace Zaly.Models.Database {
    public sealed class TeamRepository : DatabaseRepository<Team> {
        public TeamRepository(DatabaseContext context) {
            _context = context;
        }
        public override void Add(Team entity) {
            _context.Team.Add(entity);
            _context.SaveChanges();
        }
        public List<User> GetTeamUsers(Team entity) {
            return _context.User.Where(u => u.TeamId == entity.Id).ToList();
        }
        public override void Delete(int id) {
            var Team = FindById(id);
            if (Team is null) {
                return;
            }
            _context.Team.Remove(Team);
            _context.SaveChanges();
        }

        public override void Delete(Team entity) {
            _context.Team.Remove(entity);
            _context.SaveChanges();
        }

        public override Team? FindById(int id) {
            return _context.Team.Find(id);
        }

        public override List<Team> GetAll() {
            return _context.Team.ToList();
        }
        public override void Update(int id, Team entity) {
            var dbTeam = _context.Team.Find(id);
            if (dbTeam is null) {
                return;
            }
            dbTeam.Name = entity.Name;

            _context.SaveChanges();
        }
        public int GetTeamPoints(int teamId) {
            return (from u in _context.User
                    join t in _context.Team on u.TeamId equals t.Id
                    where t.Id == teamId
                    select u).Sum(u => u.Points);
        }
    }
}
