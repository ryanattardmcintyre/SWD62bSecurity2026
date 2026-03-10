using Common.Models;
using DataAccess.CustomValidators;

namespace Presentation.Models
{
    public class CreateTicketViewModel:Ticket
    {
        [TicketQtyValidator]
        public int ValidatedQty { get; set; }

    }
}
