using DataAccess.CustomValidators;
using DataAccess.Repositories;
using Presentation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 
namespace DataAccess.CustomValidators
{
    public class TicketQtyValidator : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            EventsRepository myEventRepository = (EventsRepository)
                validationContext.GetService(typeof(EventsRepository));

            var myEvent =
                 myEventRepository.GetAllEvents().
                 SingleOrDefault(x => x.Id == ((CreateTicketViewModel)validationContext.ObjectInstance).EventFK);

            if (myEvent.MaximumTickets >= (int)value)
                return ValidationResult.Success;
            else return new ValidationResult($"Only {myEvent.MaximumTickets} tickets are available for this event");
      
     
        }
    }
}
