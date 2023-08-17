using BaseballUa.BlData;
using BaseballUa.Data;
using BaseballUa.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace BaseballUa.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ICrud _categoryCrud; 
		//private List<Category> allCategories;

		public HomeController(ILogger<HomeController> logger, ICrud categoryCrud)
		{
			_logger = logger;
			//_dbContext = context;
			_categoryCrud = categoryCrud;
		}

		public IActionResult Index()
		{
			var allCategories = _categoryCrud.GetAll().ToList();
			return View(allCategories);
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
	}
}