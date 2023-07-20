using RoomBookingApp.Core.DataService;
using RoomBookingApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingApp.Persistence.Repositories
{
    public class RoomBookingService : IRoomBookingService
    {
        private readonly RoomBookingAppDbContext _Context;
        public RoomBookingService(RoomBookingAppDbContext context)
        {
                _Context= context;
        }

        public IEnumerable<Room> GetAvailableRooms(DateTime date)
        {
            return _Context.Rooms.Where(r => !r.RoomBookings.Any(x => x.Date == date)).ToList(); 
        }

        public void SaveBooking(RoomBooking roomBooking)
        {
            _Context.RoomBookings.Add(roomBooking);
            _Context.SaveChanges();
        }
    }
}
