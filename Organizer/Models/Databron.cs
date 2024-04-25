using Microsoft.AspNetCore.Mvc;

namespace Organizer.Models
{
   
        public class TeamUserViewModel
        {
            public Task<IActionResult> Teams { get; set; }
            public List<Organizer.Entities.User> Users { get; set; }
        }

    
}