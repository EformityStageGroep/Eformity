using Microsoft.Graph;
using System;


namespace Organizer.Models
{
    public class TaskViewModel
    {
        public List<Entities.Task>? Tasks { get; set; }
        public Entities.Task? Task { get; set; }
    }
}