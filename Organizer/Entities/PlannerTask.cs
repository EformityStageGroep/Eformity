using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer.Entities
{
    [Table("Tasks")]
    public class Task : IMustHaveTenant
    {
        [Key]
        public Guid id { get; set; }

        [Required]
        public string title { get; set; }

        public string? description { get; set; }

        public string priority { get; set; }

        [Required]
        [Display(Name = "Date & Time")]
        public DateTime datetime { get; set; }

        public string selectstatus { get; set; }

        [ForeignKey("Tenants")]
        public string tenant_id { get; set; }

        [ForeignKey("Users")]
        public string? user_id { get; set; }

        public Guid? team_id { get; set; }

        public Task() => id = Guid.NewGuid();
    }

    public class PageIdentifier
    {
        public string? PageValue { get; set; }
    }
}
