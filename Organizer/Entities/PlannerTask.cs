using System.ComponentModel.DataAnnotations;

namespace Organizer.Entities
{
    public class Task
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public string Name { get; set; }
        public Task() => Id = Guid.NewGuid();
    }
}