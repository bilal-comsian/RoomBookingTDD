using RoomBookingApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingApp.Core.DataService
{
    public interface IRoomBookingService
    {
        void SaveBooking(RoomBooking roomBooking);
        IEnumerable<Room> GetAvailableRooms(DateTime date);
    }
}
