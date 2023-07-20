using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using Zaly.Models;

namespace Zaly.Controllers {
	public class UserController : Controller {
		private readonly DatabaseContext _context;
		List<User> l;
		string errormes = "nothing";
        public UserController() {
			try {
				_context = new();
				l = _context.User.ToList();
			} catch (Exception ex) {
				this.errormes = ex.Message;
			}
		}
        public IActionResult Index() {
			if (l != null)
				this.ViewBag.l = l;
			this.ViewBag.e = errormes;
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
