using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer.Entities
{
    public class User
    {
        [Key]
        public string id { get; set; }
        public string tenant_id { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }

        // Foreign key property for the Role entity
        [ForeignKey("Role")]
        public Guid role_id { get; set; }

        // Navigation property for the related Role
        public Role Role { get; set; }

        public User()
        {
            Users_Teams = new HashSet<UserTeam>();
           
        }
        
        public virtual ICollection<UserTeam> Users_Teams { get; set; }
       
    }
}
