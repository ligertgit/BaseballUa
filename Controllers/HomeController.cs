using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.DTO;
using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace BaseballUa.Controllers
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
			
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public IActionResult History()
		{
			return View();
		}
	}
}