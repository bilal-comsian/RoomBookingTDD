using RoomBookingApp.Core.Enums;
using RoomBookingApp.Domain.BaseModel;
using System;
using System.Collections.Generic;

namespace RoomBookingApp.Core.Model
{
    public class RoomBookingResult : RoomBookingBase
    {
        public BookingResultFlag Flag { get; set; }
        public int? RoomBookingId { get; set; }
    }
}