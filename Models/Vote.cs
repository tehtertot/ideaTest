namespace brightIdeasTest.Models
{
    public class Vote : BaseEntity
    {
        public int VoteId { get; set; }
        public int Direction { get; set; }
        
        public int UserId { get; set; }
        public User Voter { get; set; }

        public int IdeaId { get; set; }
        public Idea Idea { get; set; }
    }
}