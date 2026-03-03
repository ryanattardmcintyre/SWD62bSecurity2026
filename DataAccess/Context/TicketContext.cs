using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
 * EntityFrameworkCore
 * EntityFrameworkCore.SqlServer
 * EntityFrameworkCore.Tools
 * EntityFrameworkCore.Abstraction
 * entiyframeworkcore.Relational
 * EntityFrameworkCore.identity
 */


namespace DataAccess.Context
{
    public class TicketContext: IdentityDbContext<IdentityUser>
    {
        public TicketContext(DbContextOptions<TicketContext> options)
            : base(options)
            { }

        public DbSet<Common.Models.Event> Events { get; set; }
        public DbSet<Common.Models.Ticket> Tickets { get; set; }


    }
}
