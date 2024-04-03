using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer.Entities
{
    [Table("Tasks")]
    public class Task
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public string Priority { get; set; }

        [Display(Name = "Date & Time")]
        public DateTime DateTime { get; set; }

        public Task() => Id = Guid.NewGuid();
        
    }
 /*   public class PageIdentifier
    {
        public string? PageValue { get; set; }
    }*/
}
