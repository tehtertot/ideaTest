using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace brightIdeasTest.Models
{
    public class Idea : BaseEntity
    {
        public int IdeaId { get; set; }

        [Required]
        [MinLength(5, ErrorMessage="Description must be at least 5 characters.")]
        public string Description { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<Vote> IdeaVotes { get; set; }

        public Idea() {
            IdeaVotes = new List<Vote>();
        }        
    }
}