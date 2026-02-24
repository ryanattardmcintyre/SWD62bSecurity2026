using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public class TicketContext: IdentityDbContext
    {
        public TicketContext(DbContextOptions<TicketContext> options)
            : base(options)
            { }


        
    
    }
}
