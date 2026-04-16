using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        
        public IActionResult Index()
        {
            //Log Levels: Trace =0 , Debug = 1, Information = 2, Warning =3, Error= 4, Critical=5

            _logger.Log(LogLevel.Information, "Went into the Home/Index method");
            try
            {
                throw new Exception("demo exception");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in Home/Index");
            }

            _logger.LogWarning("This is a warning message post error generation");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult StatusError(int code)
        {
            if (code == 404)
            {
                return Content("Page not found - Url typed is not correct");
            }

            return Content("Unhandled error ocurred - we logged it - try again later");
        }

         
    }
}
