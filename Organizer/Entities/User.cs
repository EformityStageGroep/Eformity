using System.ComponentModel.DataAnnotations;

namespace Organizer.Entities
{
    public class User
    {
        [Key]
        public string Id { get; set; }
        public string Tenant_Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<UserTeam> Users_Teams { get; set; }
    }
}