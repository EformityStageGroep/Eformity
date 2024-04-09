using Microsoft.Graph;
using System;


namespace Organizer.Models
{
    public class TaskViewModel
    {
        public List<Organizer.Entities.Task>? Tasks { get; set; }
        public Organizer.Entities.Task? Task { get; set; }
    }
}