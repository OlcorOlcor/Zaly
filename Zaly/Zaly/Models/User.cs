namespace Zaly.Models {
	public class User {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string Nickname { get; set; } = "";
        public int Points { get; set; }
        public string Password { get; set; } = "";
        public int TeamId { get; set; }
    }
}
