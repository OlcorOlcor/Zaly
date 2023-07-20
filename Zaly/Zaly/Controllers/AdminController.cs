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
		public IActionResult Delete(int Id) {
			//TODO
			return Redirect("Index");
		}
		public IActionResult Edit() {
			return View();
		}
	}
}
