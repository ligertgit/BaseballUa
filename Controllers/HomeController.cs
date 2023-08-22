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
		private readonly ICrud<Category> _categoryCrud; 

		public HomeController(ILogger<HomeController> logger, ICrud<Category> categoryCrud)
		{
			_logger = logger;
			_categoryCrud = categoryCrud;
		}

		public IActionResult Index()
		{
			var allCategories = _categoryCrud.GetAll().Select(a => CategoryToView.Convert(a)).ToList();
			return View(allCategories);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}