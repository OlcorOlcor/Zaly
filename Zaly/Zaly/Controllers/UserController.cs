using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Zaly.Models;
using Zaly.Models.Database;

namespace Zaly.Controllers
{
    public class UserController : Controller {
		private readonly IRepository<User> _userRepository;
		private readonly IRepository<Question> _questionRepository;
		private readonly IRepository<UserToQuestion> _userToQuestionRepository;
		private readonly IRepository<MultipartAnswer> _multipartAnswerRepository;
		private readonly IRepository<Team> _teamRepository;
		public UserController(IRepository<User> userRepository, IRepository<Question> questionRepository, IRepository<UserToQuestion> userToQuestionRepository, IRepository<MultipartAnswer> multipartAnswerRepository, IRepository<Team> teamRepository) {
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _userToQuestionRepository = userToQuestionRepository;
            _multipartAnswerRepository = multipartAnswerRepository;
            _teamRepository = teamRepository;
        }

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
			var questions = ((QuestionRepository)_questionRepository).GetQuestionsForGivenUser(userId);
			List<bool> completed = new();
			foreach (var question in questions) {
				var link = ((UserToQuestionRepository)_userToQuestionRepository).FindByFk(userId, question.Id);
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
			var users = _userRepository.GetAll().OrderByDescending(x => x.Points);
			var teams = _teamRepository.GetAll();
			List<int> teamPoints = new();
			foreach (var team in teams) {
				teamPoints.Add(((TeamRepository)_teamRepository).GetTeamPoints(team.Id));
			}
			ViewBag.Users = users;
			ViewBag.Teams = teams;
			ViewBag.TeamPoints = teamPoints;
            return View();
		}
		public IActionResult Profile() {
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
			
			var user = ((UserRepository)_userRepository).Login(ViewBag.LoggedInUser.Login, oldPassword);
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
			var user = ((UserRepository)_userRepository).Login(Login, Password);
			if (user is null) {
				ViewBag.LoginFailed = true;
				ViewBag.Login = Login;
				ViewBag.Logged = false;
				return View();
			}
			HttpContext.Session.SetString("login", "true");
			HttpContext.Session.SetInt32("userid", user.Id);
			if (HttpContext.Session.GetString("QuestionCodeToRegister") is not null) {
				RegisterQuestion(HttpContext.Session.GetString("QuestionCodeToRegister")!);
			}
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
            var question = ((QuestionRepository)_questionRepository).FindByCode(code);

			if (question is null || question.Multipart) {
				return RedirectToAction("Index");
			}
			var link = ((UserToQuestionRepository)_userToQuestionRepository).FindByFk((int)HttpContext.Session.GetInt32("userid")!, question.Id);
			if (link is null || link.Completed) {
				return RedirectToAction("Index");
			}
			if (question.Img is not null && question.Img != "") {
				ViewBag.ImgPath = $"/Img/{question.Img}";
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
			if (question == null || question.Multipart) {
				return RedirectToAction("Index");
			}

			var user = _userRepository.FindById((int)HttpContext.Session.GetInt32("userid")!);
			if (!Answer.ToLower().Trim().Equals(question.Answer.ToLower().Trim())) {
				ViewBag.Question = question;
				ViewBag.Message = "Nesprávná odpověď";
                if (question.Img is not null || question.Img != "") {
                    ViewBag.ImgPath = $"/Img/{question.Img}";
                }
                else {
                    ViewBag.ImgPath = "";
                }
                return View();
			}
			user!.Points += question.Points;
            _userRepository.Update(user.Id, user);
			var link = ((UserToQuestionRepository)_userToQuestionRepository).FindByFk(user.Id, question.Id);
			if (link == null) {
				return RedirectToAction("Index");
			}
			link.Completed = true;	
			_userToQuestionRepository.Update(link.Id, link);
            return RedirectToAction("Index");
		}
		[HttpGet]
		public IActionResult MultiQuestion(string code) {
			if (!CheckLogin()) {
				return RedirectToAction("Login");
			}
			var question = ((QuestionRepository)_questionRepository).FindByCode(code);
			if (question == null || !question.Multipart) {
				return RedirectToAction("Index");
			}
			var link = ((UserToQuestionRepository)_userToQuestionRepository).FindByFk((int)HttpContext.Session.GetInt32("userid")!, question.Id);
			if (link is null || link.Completed) {
				return RedirectToAction("Index");
			}
			var options = ((MultipartAnswerRepository)_multipartAnswerRepository).FindByFk(question.Id);
			//SHUFFLE LIST - TODO: extract
			Random rng = new Random();
            int n = options.Count;
            while (n > 1) {
                n--;
                int k = rng.Next(n + 1);
                var value = options[k];
                options[k] = options[n];
                options[n] = value;
            }
            if (question.Img is not null || question.Img != "") {
                ViewBag.ImgPath = $"/Img/{question.Img}";
            }
            else {
                ViewBag.ImgPath = "";
            }
            ViewBag.Question = question;
			ViewBag.Options = options;
			return View();
		}
		[HttpPost]
		public IActionResult MultiQuestion(int Id, string Answer) {
            if(!CheckLogin()) {
                return RedirectToAction("Login");
            }
            var question = _questionRepository.FindById(Id);
            if (question == null || !question.Multipart) {
                return RedirectToAction("Index");
            }

            var user = _userRepository.FindById((int)HttpContext.Session.GetInt32("userid")!);
            if (!Answer.ToLower().Trim().Equals(question.Answer.ToLower().Trim())) {
                ViewBag.Question = question;
                ViewBag.Message = "Nesprávná odpověď";
                user!.Points--;
                _userRepository.Update(user.Id, user);
                var options = ((MultipartAnswerRepository)_multipartAnswerRepository).FindByFk(question.Id);
				ViewBag.Options = options;
                if (question.Img is not null || question.Img != "") {
                    ViewBag.ImgPath = $"/Img/{question.Img}";
                }
                else {
                    ViewBag.ImgPath = "";
                }
                return View();
            }
            var link = ((UserToQuestionRepository)_userToQuestionRepository).FindByFk(user!.Id, question.Id);
            if (link == null || link.Completed) {
                return RedirectToAction("Index");
            }
            link.Completed = true;
            _userToQuestionRepository.Update(link.Id, link);
            user!.Points += question.Points;
            _userRepository.Update(user.Id, user);
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
			var question = ((QuestionRepository)_questionRepository).FindByCode(code);
			if (user is null || question is null) {
				return;
			}
			if (((UserToQuestionRepository)_userToQuestionRepository).FindByFk(user.Id, question.Id) is not null) {
				return;
			}
			var utq = new UserToQuestion();
			utq.UserId = user.Id;
			utq.QuestionId = question.Id;
			_userToQuestionRepository.Add(utq);
		}
		[HttpGet]
		public IActionResult Dud() {
			return View();
		}
		[HttpGet]
		public IActionResult Dud2() {
			return View();
		}
		[HttpGet]
		public IActionResult Dud3() {
			return View();
		}
		[HttpGet]
		public IActionResult Dud4() {
			return View();
		}
		[HttpGet]
		public IActionResult Dud5() {
			return View();
		}
	}
}
