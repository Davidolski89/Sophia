using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sophia.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Sophia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Das öffnet die Startseite
        // Wenn im Browser localhost/sophia/home aufgerufen wird, wird automatisch die Index Methode aufgerufen
        // localhost/sophia/home/Index ist das gleiche. 
        // Wenn es keine Index Methode gibt kommt eine Fehlermeldung
        // Default Controller ist HomeController deshalb wird die Methode auch bei localhost/sophia/ aufgerufen
        // Die Einstellungen für defaultController findet man in der Startup.cs bei den endpoints
        public IActionResult Index()
        {
            return View();
        }

        // localhost/sophia/home/privacy
        public IActionResult Privacy()
        {
            return View();
        }

        //ka genau was ich hier mache. könnte iwas kopiertes oder automatisch generiertes sein.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
