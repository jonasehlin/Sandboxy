using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Encodings.Web;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sandboxy.Controllers
{
	public class HelloWorldController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		// 
		// GET: /HelloWorld/Welcome/ 
		public IActionResult Welcome(string name, int ID = 1)
		{
			return View();
		}

		[HttpPost]
		public IActionResult Upload(IEnumerable<IFormFile> files)
		{


			return View("Index");
		}
	}
}
