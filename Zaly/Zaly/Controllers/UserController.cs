﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Reflection.Metadata;
using Zaly.Models;
using Zaly.Models.Database;

namespace Zaly.Controllers
{
    public class UserController : Controller {
		private readonly UserRepository _userRepository = new();
		private readonly QuestionRepository _questionRepository = new();
		private readonly UserToQuestionRepository _userToQuestionRepository = new();
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
			int userId = (int)HttpContext.Session.GetInt32("userid")!;
			var questions = _questionRepository.GetQuestionsForGivenUser(userId);
			List<bool> completed = new();
			foreach (var question in questions) {
				var link = _userToQuestionRepository.FindByFk(userId, question.Id);
				completed.Add(link!.Completed);
			}
			ViewBag.Questions = questions;
			ViewBag.Completed = completed;
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
			if (HttpContext.Session.GetString("QuestionCodeToRegister") is not null) {
				RegisterQuestion(HttpContext.Session.GetString("QuestionCodeToRegister")!);
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
		public IActionResult SimpleQuestion(string code) {
            if (!CheckLogin()) {
                return RedirectToAction("Login");
            }

			//TODO: Check if the question isn't already completed

            var question = _questionRepository.FindByCode(code);

			if (question is null) {
				return RedirectToAction("Index");
			}
			if (question.Img is not null || question.Img != "") {
				ViewBag.ImgPath = $"~/Img/{question.Img}";
			} else {
				ViewBag.ImgPath = "";
			}
			ViewBag.Question = question;
			return View();
		}
		[HttpPost]
		public IActionResult SimpleQuestion(int Id, string Answer) {
            if (!CheckLogin()) {
                return RedirectToAction("Login");
            }
			var question = _questionRepository.FindById(Id);
			if (question == null) {
				return RedirectToAction("Index");
			}

			var user = _userRepository.FindById((int)HttpContext.Session.GetInt32("userid")!);
			if (Answer != question.Answer) {
				ViewBag.Question = question;
				ViewBag.Message = "Nesprávná odpověď";
				user!.Points--;
				_userRepository.Update(user.Id, user);
				return View();
			}
			user!.Points += question.Points;
            _userRepository.Update(user.Id, user);
			var link = _userToQuestionRepository.FindByFk(user.Id, question.Id);
			if (link == null) {
				return RedirectToAction("Index");
			}
			link.Completed = true;	
			_userToQuestionRepository.Update(link.Id, link);
            return RedirectToAction("Index");
		}
		[HttpGet]
		public IActionResult RegisterQuestionToUser(string code) {
            if (!CheckLogin()) {
				HttpContext.Session.SetString("QuestionCodeToRegister", code);
				return RedirectToAction("Login");
            }
			RegisterQuestion(code);
			return RedirectToAction("Index");
        }
		private void RegisterQuestion(string code) {
			var user = _userRepository.FindById((int)HttpContext.Session.GetInt32("userid")!);
			var question = _questionRepository.FindByCode(code);
			if (user is null || question is null) {
				return;
			}
			if (_userToQuestionRepository.FindByFk(user.Id, question.Id) is not null) {
				return;
			}
			var utq = new UserToQuestion();
			utq.UserId = user.Id;
			utq.QuestionId = question.Id;
			_userToQuestionRepository.Add(utq);
		}
	}
}
