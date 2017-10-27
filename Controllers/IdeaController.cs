using brightIdeasTest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace brightIdeasTest.Controllers
{
    public class IdeaController : Controller
    {
        private IdeasContext _context;

        public IdeaController(IdeasContext context) {
            _context = context;
        }

        [HttpGet]
        [Route("bright_ideas")]
        public IActionResult Dashboard() {
            if (IsLoggedIn()) {
                ViewBag.alias = HttpContext.Session.GetString("alias");
                ViewBag.id = HttpContext.Session.GetInt32("userId");

                ViewBag.allIdeas = _context.Ideas.Include(i => i.IdeaVotes).Include(i => i.User);
                return View();
            }
            TempData["error"] = "You must be logged in to view.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Route("bright_ideas/create")]
        public IActionResult AddIdea(string desc) {
            Idea idea = new Idea {
                Description = desc == null ? "" : desc.Trim(),
                UserId = (int)HttpContext.Session.GetInt32("userId")
            };

            if (TryValidateModel(idea)) {
                _context.Ideas.Add(idea);
                _context.SaveChanges();
            }
            else {
                TempData["error"] = "Description must be at least 5 characters.";
            }
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("bright_ideas/{id}/show")]
        public IActionResult Show(int id) {
            if (IsLoggedIn()) {
                Idea thisIdea = _context.Ideas
                                .Include(i => i.User)
                                .Include(i => i.IdeaVotes)
                                    .ThenInclude(iv => iv.Voter)
                                .SingleOrDefault(i => i.IdeaId == id);
                List<Vote> upvoters = thisIdea.IdeaVotes.Where(iv => iv.Direction == 1).GroupBy(iv => iv.UserId).Select(iv => iv.First()).ToList();
                List<Vote> downvoters = thisIdea.IdeaVotes.Where(iv => iv.Direction == -1).GroupBy(iv => iv.UserId).Select(iv => iv.First()).ToList();
                ViewBag.idea = thisIdea;
                ViewBag.upvoters = upvoters;
                ViewBag.downvoters = downvoters;
                return View();
            }
            TempData["error"] = "You must be logged in.";
            return RedirectToAction("Index", "Home");
        }
        
        [HttpGet]
        [Route("bright_ideas/{id}/{dir}")]
        public IActionResult Vote(int id, string dir) {
            if (IsLoggedIn()) {
                if (dir != "up" && dir != "down") {
                    return RedirectToAction("Dashboard");
                }
                Vote v = new Vote {
                    UserId = (int)HttpContext.Session.GetInt32("userId"),
                    IdeaId = id,
                    Direction = dir == "up" ? 1 : -1
                };
                _context.Votes.Add(v);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            TempData["error"] = "You must be logged in.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("bright_ideas/{id}/delete")]
        public IActionResult delete(int id) {
            if (IsLoggedIn()) {
                Idea thisIdea = _context.Ideas.SingleOrDefault(i => i.IdeaId == id && i.UserId == (int)HttpContext.Session.GetInt32("userId"));
                if (thisIdea != null) {
                    _context.Ideas.Remove(thisIdea);
                    _context.SaveChanges();
                }
                else {
                    TempData["error"] = "Error attempting to delete idea.";
                }
                return RedirectToAction("Dashboard");
            }
            TempData["error"] = "You must be logged in.";
            return RedirectToAction("Index", "Home");
        }

        private bool IsLoggedIn() {
            return HttpContext.Session.GetInt32("userId") != null;
        }
    }
}