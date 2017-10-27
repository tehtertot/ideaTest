using System;
using brightIdeasTest.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace brightIdeasTest.Controllers
{
    public class HomeController : Controller
    {
        private IdeasContext _context;

        public HomeController(IdeasContext context) {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel rvm) {
            if (ModelState.IsValid) {
                //check for existing user with name/alias
                List<User> existingEm = _context.Users.Where(u => u.Email == rvm.Email).ToList();
                List<User> existingAl = _context.Users.Where(u => u.Alias == rvm.Alias).ToList();

                //if email or alias is not unique
                if (existingEm.Count > 0 || existingAl.Count > 0) {
                    if (existingEm.Count > 0) {
                        ModelState.AddModelError("UniqueEmail", "That email has already been registered");
                    }
                    if (existingAl.Count > 0) {
                        ModelState.AddModelError("UniqueAlias", "That alias has already been registered");
                    }
                }
                //add the user to the db
                else {
                    User newU = new User {
                        Name = rvm.Name,
                        Alias = rvm.Alias,
                        Email = rvm.Email,
                        Password = rvm.Password
                    };
                    _context.Users.Add(newU);
                    _context.SaveChanges();
                    //set session variables
                    HttpContext.Session.SetInt32("userId", newU.UserId);
                    HttpContext.Session.SetString("alias", newU.Alias);
                    return RedirectToAction("Dashboard", "Idea");
                }
            }
            return View("Index");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string loginval, string loginpassword) {
            User login;
            //get user by email
            if (loginval.Contains("@")) {
                login = _context.Users.SingleOrDefault(u => u.Email == loginval);
            }
            //get user by alias
            else {
                login = _context.Users.SingleOrDefault(u => u.Alias == loginval);
            }

            //check password
            if (login != null) {
                if (login.Password == loginpassword) {
                    HttpContext.Session.SetInt32("userId", login.UserId);
                    HttpContext.Session.SetString("alias", login.Alias);
                    return RedirectToAction("Dashboard", "Idea");
                }
                else {
                    ViewBag.pwerror = "Password is incorrect.";
                }
            }
            else {
                ViewBag.loginerror = "Email/Alias not found.";
            }
            return View("Index");
        }

        [HttpGet]
        [Route("users/{id}")]
        public IActionResult Show(int id) {
            int? userid = HttpContext.Session.GetInt32("userId");
            // if (userid == null) {
            //     TempData["error"] = "You must be logged in.";
            //     return RedirectToAction("Index");
            // }
            User user = _context.Users.Include(u => u.MyVotes).Include(u => u.MyIdeas).Single(u => u.UserId == id);
            ViewBag.user = new UserView { Alias = user.Alias,
                                  Name = user.Name,
                                  Email = user.Email,
                                  NumIdeas = user.MyIdeas.Count, 
                                  NumIdeaUps = user.MyIdeas.Sum(i => i.IdeaVotes.Where(v => v.Direction == 1).Count()),
                                  NumIdeaDowns = user.MyIdeas.Sum(i => i.IdeaVotes.Where(v => v.Direction == -1).Count()),
                                  NumIdeasVoted = user.MyVotes.GroupBy(v => v.IdeaId).Count(),
                                  NumUpVotes = user.MyVotes.Count(i => i.Direction == 1),
                                  NumDownVotes = user.MyVotes.Count(i => i.Direction == -1) };
            return View();
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
