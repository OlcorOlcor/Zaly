using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Reflection.Metadata;
using Zaly.Models;
using Zaly.Models.Database;

namespace Zaly.Controllers
{
    public class UserController : Controller {
		private readonly UserRepository _userRepository = new();
		private readonly QuestionRepository _questionRepository = new();
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
            if (!CheckLogin()) {
                return RedirectToAction("Login");
            }
            return View();
		}
		[HttpPost]
		public IActionResult ChangePassword(string oldPassword, string newPassword, string newPasswordAgain) {
            if (!CheckLogin()) {
				return RedirectToAction("Login");
			}
			
			var user = _userRepository.Login(ViewBag.LoggedInUser.Login, oldPassword);
            if (user is null) {
				return RedirectToAction("ChangePassword");
			}
			if (newPassword != newPasswordAgain) {
				return RedirectToAction("ChangePassword");
			}					
			Hasher pm = new Hasher();
			var hashedPassword = pm.HashPassword(newPassword, out string salt);
			user.Password = hashedPassword + salt;
			_userRepository.Update(user.Id, user);
			return RedirectToAction("Index");
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
				ViewBag.Logged = false;
				return View();
			}
			HttpContext.Session.SetString("login", "true");
			HttpContext.Session.SetInt32("userid", user.Id);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public IActionResult Logout() {
            HttpContext.Session.Remove("login");
            HttpContext.Session.Remove("adminid");
            return RedirectToAction("Login");
        }
		[HttpGet]
		public IActionResult QuestionSimple(string code) {
            if (!CheckLogin()) {
                return RedirectToAction("Login");
            }
            var question = _questionRepository.FindByCode(code);

			if (question is null) {
				return RedirectToAction("Index");
			}
			if (question.Img is not null || question.Img != "") {
				ViewBag.ImgPath = $"~/Img/{question.Img}";
			}
			ViewBag.Question = question;
			return View();
		}
		[HttpPost]
		public IActionResult QuestionSimple(int Id, string Answer) {
            if (!CheckLogin()) {
                return RedirectToAction("Login");
            }
			var question = _questionRepository.FindById(Id);
			if (question == null) {
				return RedirectToAction("Index");
			}

			if (Answer != question.Answer) {
				ViewBag.Question = question;
				ViewBag.Message = "Nesprávná odpověď";
				return View();
			}
			var user = _userRepository.FindById((int)HttpContext.Session.GetInt32("userid")!);
			user!.Points += question.Points;
			//TODO: make question complete for given user
			return RedirectToAction("Index");
		}
	}
}
