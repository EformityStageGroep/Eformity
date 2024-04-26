using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer.Entities
{
    public class Team
    {
        [Key]
        public string Team_Id { get; set; }
        
        public string UserId { get; set; }
    
        // Navigation property for the users associated with the team
        public virtual ICollection<UserTeam> Users_Teams { get; set; }
    }
    public class UserTeam
    {
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Team")]
        public string Team_Id { get; set; }
        public virtual Team Team { get; set; }
    }
}
