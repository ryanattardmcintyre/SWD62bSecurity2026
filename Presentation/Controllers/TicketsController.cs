using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {

        [HttpGet]
        public IActionResult Buy(int id)
        {
            CreateTicketViewModel model = new CreateTicketViewModel();
            model.EventFK = id;

            //a check to return the event's data
            //+ tickets data for the event and check
            //whether the bought tickets exceed the maximum tickets for the event or not, if it does, then we can show a message to the user that the event is sold out.
            //in which case you don't allow the user to continue
            //buying tickets because its sold out

            return View(model);
        }

        [HttpPost]
        public IActionResult Buy(CreateTicketViewModel data)
        {
            if(ModelState.IsValid == false)
            {
                return View(data);
            }
            //save the ticket to the database

            Ticket ticket = new Ticket
            {
                EventFK = data.EventFK,
                Name = data.Name,
                Surname = data.Surname,
                IdCard = data.IdCard,
                Quantity = data.ValidatedQty,
                Status = Status.Paid
            };

            //add to the database....

            return View();
        }
    }
}
