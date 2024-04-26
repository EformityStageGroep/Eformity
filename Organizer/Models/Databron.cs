using Microsoft.AspNetCore.Mvc;

namespace Organizer.Models
{

    public class ParentViewModel
    {
        public List<Entities.User> Users { get; set; }
        public List<Entities.Team> Teams { get; set; }
    }


}