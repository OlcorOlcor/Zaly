using Microsoft.AspNetCore.Mvc;
using Zaly.Models;

namespace Zaly.Controllers {
	public class AdminController : Controller {
		readonly private UserRepository _userRepository = new();
		readonly private AdminRepository _adminRepository = new();
		public IActionResult Index() {
			this.ViewBag.Users = _userRepository.GetAll();
			return View();
		}
		[HttpGet]
		public IActionResult AddUser() {
			return View();
		}
		[HttpPost]
		public IActionResult AddUser(User user) {
			PasswordManager pm = new PasswordManager();
			var hashedPassword = pm.HashPassword(user.Name.ToLower() + user.Surname.ToLower(), out string salt);
			user.Login = user.Name.ToLower() + user.Surname.ToLower();
			user.Password = hashedPassword + salt;
			_userRepository.Add(user);
			return RedirectToAction("Index");
		}
		public IActionResult DeleteUser(int Id) {
			_userRepository.Delete(Id);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public IActionResult EditUser(int Id) {
			User user = _userRepository.FindById(Id)!;
			if (user == null) {
				return RedirectToAction("Index");
			}
			this.ViewBag.User = user;
			return View();
		}
		[HttpPost]
		public IActionResult EditUser(User user) {
			_userRepository.Update(user.Id, user);
			return RedirectToAction("Index");
		}
		[HttpGet]
		public IActionResult ChangePoints(int Id, int diff) {
			var user = _userRepository.FindById(Id);
			if (user == null) {
				return RedirectToAction("Index");
			}
			user.Points += diff;
			_userRepository.Update(Id, user);
			return RedirectToAction("Index");
		}
	}
}
