using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace brightIdeasTest.Models
{
    public class IdeaView : BaseEntity
    {
        public int IdeaId { get; set; }
        public string Creator { get; set; }
        public string Description { get; set; }
        public User User { get; set; }
        public List<User> Upvoters { get; set; }
        public List<User> Downvoters { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }

        public IdeaView() {
            Upvoters = new List<User>();
            Downvoters = new List<User>();
        }        
    }
}