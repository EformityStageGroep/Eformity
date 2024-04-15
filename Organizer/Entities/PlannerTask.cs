using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer.Entities
{
    [Table("Tasks")]
    public class Task : IMustHaveTenant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Priority { get; set; }

        [Display(Name = "Date & Time")]
        public DateTime DateTime { get; set; }


        public string TenantId { get; set; }


        public string SelectStatus { get; set; }

        public Task() => Id = Guid.NewGuid();
        
    }
    
 /*   public class PageIdentifier
    {
        public string? PageValue { get; set; }
    }*/
}
