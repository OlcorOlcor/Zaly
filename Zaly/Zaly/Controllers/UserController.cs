using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using Zaly.Models;

namespace Zaly.Controllers {
	public class UserController : Controller {
		private readonly UserRepository _userRepository = new();
        public UserController() {
		}
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
	}
}
