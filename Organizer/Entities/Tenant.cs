using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Organizer.Entities
{
    public class Tenant
    {
        [Key]
        public string id { get; set; }

        [Required]
        public string name { get; set; }
    }
}