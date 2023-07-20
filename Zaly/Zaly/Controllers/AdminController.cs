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
			_userRepository.Add(user);
			return Redirect("Index");
		}
		public IActionResult DeleteUser(int Id) {
			_userRepository.Delete(Id);
			return Redirect("Index");
		}
		[HttpGet]
		public IActionResult EditUser(int Id) {
			User user = _userRepository.FindById(Id)!;
			if (user == null) {
				return Redirect("Index");
			}
			this.ViewBag.User = user;
			return View();
		}
		[HttpPost]
		public IActionResult EditUser(User user) {
			_userRepository.Update(user.Id, user);
			return Redirect("Index");
		}
	}
}
