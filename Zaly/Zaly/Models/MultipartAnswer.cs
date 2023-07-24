namespace Zaly.Models {
    public class MultipartAnswer {
        public int Id { get; set; }
        public string Answer { get; set; }
        public bool Correct { get; set; }
        public int QuestionId { get; set; }
    }
}
