using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{

    //Validators
    //e.g. [Required], [Range(1, 100)], [StringLength(50)], [EmailAddress], [RegularExpression]
    //     [CustomValidation(typeof(MyValidator), "ValidateEventName")] // Custom validation example

    //force/trigger validation on the client side

    public partial class Event
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        
        public int Id { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage ="Please input an event name")]
        [StringLength(100)]
        [RegularExpression(@"^[A-Za-z0-9 ]+$", ErrorMessage = "Event name can only contain letters, numbers, and spaces")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Please input a price")]
        [Range(0, 10000, ErrorMessage = "Price must be greater than 0 but less than 10000; if it is higher" +
                                            " contact admin@events.com")]
        public double Price { get; set; }
       
        public bool Public { get; set; }

        [Range(0, 100000, ErrorMessage = "Tickets value must be greater than 0 but less than 10000; " +
                                            "if it is higher" + " contact admin@events.com")]
        public int MaximumTickets { get; set; }
        public string FilePath { get; set; }

        [EmailAddress(ErrorMessage = "Please input a valid email address")]
        public string Organiser { get; set; }
    }
}
