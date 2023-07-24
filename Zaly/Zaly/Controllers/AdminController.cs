using Microsoft.AspNetCore.Mvc;
using Zaly.Models;
using Zaly.Models.Database;

namespace Zaly.Controllers
{
    public class AdminController : Controller {
		readonly private UserRepository _userRepository = new();
		readonly private AdminRepository _adminRepository = new();
        private bool CheckLogin() {
            if (HttpContext.Session.GetString("login") != "true") {
                ViewBag.Logged = false;
                return false;
            }
            ViewBag.LoggedInAdmin = _adminRepository.FindById((int)HttpContext.Session.GetInt32("adminid")!);
            ViewBag.Logged = true;
            return true;
        }

        public IActionResult Index() {
            if (!CheckLogin()) {
                return RedirectToAction("Login");
            }
            this.ViewBag.Users = _userRepository.GetAll();
			return View();
		}
		[HttpGet]
		public IActionResult AddUser() {
            if (!CheckLogin()) {
                return RedirectToAction("Login");
            }
            return View();
		}
		[HttpPost]
		public IActionResult AddUser(User user) {
            if (!CheckLogin()) {
                return RedirectToAction("Login");
            }
            PasswordManager pm = new PasswordManager();
			var hashedPassword = pm.HashPassword(user.Name.ToLower() + user.Surname.ToLower(), out string salt);
			user.Login = user.Name.ToLower() + user.Surname.ToLower();
			user.Password = hashedPassword + salt;
			_userRepository.Add(user);
			return RedirectToAction("Index");
		}
		public IActionResult DeleteUser(int Id) {
            if (!CheckLogin()) {
                return RedirectToAction("Login");
            }
            _userRepository.Delete(Id);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public IActionResult EditUser(int Id) {
            if (!CheckLogin()) {
                return RedirectToAction("Login");
            }
            User user = _userRepository.FindById(Id)!;
			if (user == null) {
				return RedirectToAction("Index");
			}
			this.ViewBag.User = user;
			return View();
		}
		[HttpPost]
		public IActionResult EditUser(User user) {
            if (!CheckLogin()) {
                return RedirectToAction("Login");
            }
            _userRepository.Update(user.Id, user);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public IActionResult ChangePoints(int Id, int diff) {
            if (!CheckLogin()) {
                return RedirectToAction("Login");
            }
            var user = _userRepository.FindById(Id);
			if (user == null) {
				return RedirectToAction("Index");
			}
			user.Points += diff;
			_userRepository.Update(Id, user);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public IActionResult Login() {
            if (!CheckLogin()) {
                ViewBag.LoginFailed = false;
                ViewBag.LoggedInAdmin = null;
                return View();
            }
            else {
                return RedirectToAction("Index");
            }	
        }
        [HttpPost]
        public IActionResult Login(string Login, string Password) {
            var admin = _adminRepository.Login(Login, Password);
            if (admin is null) {
                ViewBag.LoginFailed = true;
                ViewBag.Login = Login;
                ViewBag.Logged = false;
                return View();
            }
            HttpContext.Session.SetString("login", "true");
            HttpContext.Session.SetInt32("adminid", admin.Id);
            return RedirectToAction("Index");
        }
    }
}
