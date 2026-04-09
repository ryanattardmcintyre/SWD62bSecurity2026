using Common.Models;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Presentation.ActionFilters
{
    public class EventOrganizerPermissionFilter: IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //checking whether the user who is authenticated is the organizer of the event or not
                string userEmail = context.HttpContext.User.Identity.Name;
                int eventId = int.Parse(context.RouteData.Values["id"].ToString());


            //query the database to get the event with the eventId and check whether the organizer email of that event is the same as the user email or not
            //if not, then we can return a 403 forbidden status code

            var _eventsRepository = 
                context.HttpContext.RequestServices.GetService<EventsRepository>();


            //this is just a pseudo code, you need to implement the actual logic to query the database and check the organizer email
            Event? e = _eventsRepository.GetAllEvents().SingleOrDefault(x=>x.Id ==eventId);

            if(e.Organiser != userEmail)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }

    public class HasEventOrganizerPermissionAttribute: TypeFilterAttribute
    {
        public HasEventOrganizerPermissionAttribute(): base(typeof(EventOrganizerPermissionFilter))
        {

        }
    }
}
