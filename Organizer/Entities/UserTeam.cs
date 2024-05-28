using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer.Entities
{
    public class UserTeam
    {
        [ForeignKey("User")]
        public string user_id { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Team")]
        public Guid team_id { get; set; }
        public virtual Team Team { get; set; }
    }
}
