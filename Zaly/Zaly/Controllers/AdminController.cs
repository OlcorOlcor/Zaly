using Microsoft.AspNetCore.Mvc;
using Zaly.Models;

namespace Zaly.Controllers {
	public class AdminController : Controller {
		readonly private DatabaseContext _context = new();
		public IActionResult Index() {
			this.ViewBag.Users = _context.User.ToList();
			return View();
		}
		public IActionResult Add() {
			return View();
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
