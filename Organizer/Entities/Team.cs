using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer.Entities
{
    public class Team
    {
        [Key]
        public Guid id { get; set; }
        
        public string tenant_id { get; set; }
        public string title { get; set; }
    
        // Navigation property for the users associated with the team
        public virtual ICollection<UserTeam> Users_Teams { get; set; }
    }
    public class UserTeam
    {
        [ForeignKey("User")]
        public string User_Id { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Team")]
        public Guid Team_Id { get; set; }
        public virtual Team Team { get; set; }
    }
}
