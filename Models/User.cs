using System.Collections.Generic;

namespace brightIdeasTest.Models
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Idea> MyIdeas { get; set; }
        public List<Vote> MyVotes { get; set; }

        public User() {
            MyIdeas = new List<Idea>();
            MyVotes = new List<Vote>();
        }
    }
}