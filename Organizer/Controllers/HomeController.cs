using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Graph;
using Organizer.Models;
using Organizer.Services;
using System.Diagnostics;

namespace Organizer.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        SqlConnection con = new SqlConnection();
        List<Databron> Databronnen = new List<Databron>();
        private readonly ILogger<HomeController> _logger;
        private readonly IGraphClientService _graphClientService;
        private readonly GraphServiceClient graphServiceClient;

        public HomeController(ILogger<HomeController> logger, IGraphClientService graphClientService)
        {
            _logger = logger;
            _graphClientService = graphClientService;
            graphServiceClient = _graphClientService.GetGraphServiceClient();
            con.ConnectionString = Organizer.Properties.Resources.ConnectionString;
        }

        public IActionResult Index()
        {
            //FetchData();
            return View(Databronnen);
        }
        /*private void FetchData()
        {
            if (Databronnen.Count > 0)
            {
                Databronnen.Clear();
            }
            try
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT TOP (1000) [MigrationId],[ProductVersion] FROM [dbo].[__EFMigrationsHistory]";
                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    Databronnen.Add(new Databron()
                    {
                        Gmail = dr["Gmail"].ToString(),
                        Facebook = dr["Facebook"].ToString()
                    });
                }
                con.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }*/
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}