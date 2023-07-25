using System.ComponentModel.DataAnnotations.Schema;

namespace Zaly.Models {
    public class Question {
        public int Id { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string Text { get; set; } = "";
        public int Points { get; set; }
        public bool Multipart { get; set; }
        public string? Img { get; set; } = "";
        public string Answer { get; set; } = "";

        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
