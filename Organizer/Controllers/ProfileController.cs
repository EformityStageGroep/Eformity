using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Graph;
using Organizer.Models;
using Organizer.Services;
using System.Diagnostics;

namespace Organizer.Controllers
{
   
    public class ProfileController : Controller
    {
        public ActionResult Index()
        { 

            // Pass the user profile data to the view
            return View();
        }

        
    }
}
