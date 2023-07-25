namespace Zaly.Models {
    public class UserToQuestion {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }
        public bool Completed { get; set; } = false;
    }
}
