using System.ComponentModel.DataAnnotations;

namespace Organizer.Entities
{
    public class Team
    {
        [Key]
        public Guid id { get; set; }

        public string tenant_id { get; set; }

        [Required]
        public string title { get; set; }

        // Navigation property for the users associated with the team
        public virtual ICollection<UserTeam> Users_Teams { get; set; }
    }
}
