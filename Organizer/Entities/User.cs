using System.ComponentModel.DataAnnotations;

namespace Organizer.Entities
{
    public class User
    {
        [Key]
        public string id { get; set; }
        public string tenant_id { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }

        public User()
        {
            Users_Teams = new HashSet<UserTeam>();
        }

        public virtual ICollection<UserTeam> Users_Teams { get; set; }
    }
}
