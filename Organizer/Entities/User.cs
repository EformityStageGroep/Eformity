using System.ComponentModel.DataAnnotations;

namespace Organizer.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Id2 { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public User() => Id = Guid.NewGuid();
    }
}