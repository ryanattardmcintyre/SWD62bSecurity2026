using Common.Models;
using DataAccess.Context;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class EventsRepository
    {
        private readonly TicketContext _context;
        public EventsRepository(TicketContext context) {
            _context = context;
        }

        public IQueryable<Event> GetAllEvents() {
            return _context.Events;
        }

        public void CreateEvent(Event newEvent) {
            
            _context.Events.Add(newEvent);
            _context.SaveChanges();
        }

        /* public List<Event> GetEvents(string keyword)
         {
             SqlCommand cmd = new SqlCommand("procGetEvents");
        cmd.CommandType= CommandTypes.StoredProcedure;

             cmd.Parameters.AddWithValue("@keyword", keyword);

             List<Event> events = new List<Event>();

             var reader = cmd.ExecuteReader();
             while(reader.Read())
             {
                 events.Add(new Event
                 {
                     Id = reader.GetInt32(0),
                     Name = reader.GetString(1),
                     Price = reader.GetDouble(2),
                     Public = reader.GetBoolean(3),
                     MaximumTickets = reader.GetInt32(4),
                     FilePath = reader.GetString(5),
                     Organiser = reader.GetString(6)
                 });
             }
         }*/
    }
}
