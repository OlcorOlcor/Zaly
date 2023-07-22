using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Reflection.Metadata;
using Zaly.Models;

namespace Zaly.Controllers {
	public class UserController : Controller {
		private readonly UserRepository _userRepository = new();
        public IActionResult Index() {
            return View();
		}
		public IActionResult Snake() {
			return View();
		}
		public IActionResult Leaderboard() {
			return View();
		}
		public IActionResult Profile() {
			return View();
		}
		public IActionResult Player() {
			return View();
		}

		[HttpGet]
		public IActionResult Login() {
            ViewBag.LoginFailed = false;
            return View();
		}
		[HttpPost]
		public IActionResult Login(string Login, string Password) {
			var user = _userRepository.Login(Login, Password);
			if (user is null) {
				ViewBag.LoginFailed = true;
				ViewBag.Login = Login;
				return View();
			}
			return RedirectToAction("Index");
		}
	}
}
