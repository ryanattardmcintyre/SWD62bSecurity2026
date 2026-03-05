using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public enum Status { Paid, Cancelled, Used }
    public class Ticket
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [ForeignKey("Event")]
        public int EventFK { get; set; }
        public virtual Event Event { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please input your name")]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please input your surname")]
        public string Surname { get; set; }

        [Required]
        [RegularExpression("^[a-z][A-Z][0-9]*$", ErrorMessage ="No symbols, only numbers and a letter")]
        [StringLength(10)]
        public string IdCard { get; set; }

        public Status Status { get; set; }

        [Required]
        public int Quantity { get; set; }


    }
}
