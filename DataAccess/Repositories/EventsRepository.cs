using Common.CustomExceptions;
using Common.Models;
using DataAccess.Context;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        public EventsRepository(TicketContext context, IConfiguration configuration) {
            _context = context;
            _configuration = configuration;
        }


        //this is a method which we must ensure it is run by the lesser privileged db login!
        //this ensures that if an sqli attack is successful,
        //the damage is limited as the attacker will be using a lesser privileged login where Delete or Create are denied
        public IQueryable<Event> GetAllEvents() {
   
            string connectionString = _configuration.GetConnectionString("UserConnection");
            if (!string.IsNullOrEmpty(connectionString))
            {
                _context.Database.SetConnectionString(connectionString);
            }

            //querying or any command which is run on the database after this line, will be running
            //with least privilege login credentials,
            //which is a good security practice to limit the damage of an sqli attack

            return _context.Events;
        }

        public void CreateEvent(Event newEvent) {
            if(GetAllEvents().Any(e=>e.Name == newEvent.Name))
            {
                throw new CreateEventException();
            }
             string connectionString = _configuration.GetConnectionString("AdminConnection");
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


        public void DeleteEvent(int id) {

            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrEmpty(connectionString))
            {
                _context.Database.SetConnectionString(connectionString);
            }

            var eventToDelete = _context.Events.Find(id);
            if (eventToDelete != null)
            {
                _context.Events.Remove(eventToDelete);
                _context.SaveChanges();
            }
            

        }   
    }
}
