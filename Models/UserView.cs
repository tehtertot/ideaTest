namespace brightIdeasTest.Models
{
    public class UserView : BaseEntity
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }

        public int NumIdeas { get; set; }
        public int NumIdeaUps { get; set; }
        public int NumIdeaDowns { get; set; }

        public int NumIdeasVoted { get; set; }
        public int NumUpVotes { get; set; }
        public int NumDownVotes { get; set; }
    }
}