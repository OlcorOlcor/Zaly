using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Reflection.Metadata;
using Zaly.Models;

namespace Zaly.Controllers {
	public class UserController : Controller {
		private readonly UserRepository _userRepository = new();
		private bool CheckLogin() {
            if (HttpContext.Session.GetString("login") != "true") {
				ViewBag.Logged = false;
				return false;
            }
			ViewBag.LoggedInUser = _userRepository.FindById((int)HttpContext.Session.GetInt32("userid")!);
			ViewBag.Logged = true;
			return true;
        }
        public IActionResult Index() {
			if (!CheckLogin()) {
				return RedirectToAction("Login");
			}
            return View();
		}
		public IActionResult Snake() {
            if (!CheckLogin()) {
                return RedirectToAction("Login");
            }
            return View();
		}
		public IActionResult Leaderboard() {
            if (!CheckLogin()) {
                return RedirectToAction("Login");
            }
            return View();
		}
		public IActionResult Profile() {
            if (!CheckLogin()) {
                return RedirectToAction("Login");
            }
            return View();
		}
		public IActionResult Player() {
            if (!CheckLogin()) {
                return RedirectToAction("Login");
            }
            return View();
		}
		[HttpGet]
		public IActionResult ChangePassword() {
			return View();
		}
		[HttpGet]
		public IActionResult Login() {
            if (!CheckLogin()) {
				ViewBag.LoginFailed = false;
				ViewBag.LoggedInUser = null;
				return View();
            } else {
				return RedirectToAction("Index");
			}
		}
		[HttpPost]
		public IActionResult Login(string Login, string Password) {
			var user = _userRepository.Login(Login, Password);
			if (user is null) {
				ViewBag.LoginFailed = true;
				ViewBag.Login = Login;
				return View();
			}
			HttpContext.Session.SetString("login", "true");
			HttpContext.Session.SetInt32("userid", user.Id);
			return RedirectToAction("Index");
		}
	}
}
