using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Zaly.Models {
	public class User {
        public int Id { get; set; }
        [RegexStringValidator("^[A-Z][a-z]+$")]
        [Required]
        public string Name { get; set; } = "";
        [RegexStringValidator("^[A-Z][a-z]+$")]
        [Required]
        public string Surname { get; set; } = "";
        [MinLength(1)]
        public string Login { get; set; } = "";
        public int Points { get; set; }
        public string Password { get; set; } = "";
        public int TeamId { get; set; }
    }
}
