using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Organizer.Entities
{

    public class Role
    {
        [Key]
        public Guid id { get; set; }

        [ForeignKey("Tenants")]
        public string tenant_id { get; set; }

        public string title { get; set; }

        public bool create_team { get; set; }

        public bool assign_task { get; set; }
    }
}