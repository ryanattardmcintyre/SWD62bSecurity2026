using DataAccess.CustomValidators;
using DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public partial class Event
    {
        [TicketStockValidator]
        public int MaximumTickets { get; set; }
    }
}

namespace DataAccess.CustomValidators
{
    public class TicketStockValidator : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            EventsRepository myEventRepository = (EventsRepository)
                validationContext.GetService(typeof(EventsRepository));

           var myEvent =
                myEventRepository.GetAllEvents().
                SingleOrDefault(x=>x.Id == (int)validationContext.Items["EventId"]);

            if (myEvent.MaximumTickets >= (int)value)
                return ValidationResult.Success;
            else return new ValidationResult($"Only {myEvent.MaximumTickets} tickets are available for this event");
      
            //getting the event
            //checking the maximum no of tickets > value
        }
    }
}
