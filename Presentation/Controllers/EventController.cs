using Common.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace Presentation.Controllers
{
    public class EventController : Controller
    {
        private EventsRepository _eventsRepository;
        public EventController(EventsRepository eventsRepository) {
         _eventsRepository = eventsRepository;
        }
        public IActionResult Index()
        {
            var list = _eventsRepository.GetAllEvents().Where(e => e.Public == true).Select(e=> new Event
            {
                Id = e.Id,
                Name = e.Name,
                Price = e.Price,
                MaximumTickets = e.MaximumTickets,
                FilePath = e.FilePath,
            }).ToList();

            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken] //generate a server side nonce token and validate it with 
                                    //the token that has to be generated on the client side as well
        public IActionResult Create(Event e)
        {
            //validate
            //sanitize
            e.Organiser = User.Identity.Name;
            e.FilePath = "";

            string regex = "^[a-z][A-Z][0-9]*$";
            if(!System.Text.RegularExpressions.Regex.IsMatch(e.Name, regex))
            {
                ModelState.AddModelError("Name", "Event name contains invalid characters.");
                return View(e);
            }

            if (e.Name.Contains("<") || e.Name.Contains(">"))
            {
                ModelState.AddModelError("Name", "Event name cannot contain HTML tags.");
                return View(e);
            }

            e.Name = WebUtility.HtmlEncode(e.Name);

            _eventsRepository.CreateEvent(e);
            TempData["success"] = "Event created successfully!";
            return RedirectToAction("Index");
        }
    }
}
