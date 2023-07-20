using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using RoomBookingApp.Core.Model;
using RoomBookingApp.Core.Processor;
using RoomBookingApp.Core.DataService;
using Moq;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Domain;

namespace RoomBookingApp.Core
{
    public class RoomBookingRequestProcessorTest
    {
        private readonly RoomBookingRequestProcessor _processor;
        private RoomBookingRequest _request;
        private Mock<IRoomBookingService> _roomBookingServiceMock;
        private List<Room> _availableRooms;
        public RoomBookingRequestProcessorTest()
        {
            //Arrange

            _request = new RoomBookingRequest
            {
                FullName = "Test Name",
                Email = "test@request.com",
                Date = new DateTime(2021, 10, 20)
            };
            _availableRooms = new List<Room> { new Room() };
            _roomBookingServiceMock = new Mock<IRoomBookingService>();
            _roomBookingServiceMock.Setup(r => r.GetAvailableRooms(_request.Date))
                .Returns(_availableRooms);
            _processor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);
        }
        [Fact]
        public void Should_Return_Room_Booking_Response_With_Request_Value()
        {
            //Act
            RoomBookingResult result = _processor.BookRoom(_request);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(_request.FullName, result.FullName);
            Assert.Equal(_request.Email, result.Email);
            Assert.Equal(_request.Date, result.Date);

            result.ShouldNotBeNull();
            result.FullName.ShouldBe(_request.FullName);
            result.Email.ShouldBe(_request.Email);
            result.Date.ShouldBe(_request.Date);
        }
        [Fact]
        public void Should_Throw_Exception_For_Null_Request()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _processor.BookRoom(null));
            exception.ParamName.ShouldBe("bookingRequest");
        }

        [Fact]
        public void Should_Save_Room_Booking_Request()
        {
            RoomBooking saveBooking = null;
            _roomBookingServiceMock.Setup(x => x.SaveBooking(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(booking =>
                {
                    saveBooking = booking;
                });
            _processor.BookRoom(_request);
            _roomBookingServiceMock.Verify(x => x.SaveBooking(It.IsAny<RoomBooking>()), Times.Once);
            saveBooking.ShouldNotBeNull();
            saveBooking.FullName.ShouldBe(_request.FullName);
            saveBooking.Email.ShouldBe(_request.Email);
            saveBooking.Date.ShouldBe(_request.Date);
        }
        [Fact]
        public void Should_Not_Save_Room_Booking_Request_If_None_Available()
        {
            _availableRooms.Clear();
            _processor.BookRoom(_request);
            _roomBookingServiceMock.Verify(x => x.SaveBooking(It.IsAny<RoomBooking>()), Times.Never);
        }
        [Theory]
        [InlineData(BookingResultFlag.Failure, false)]
        [InlineData(BookingResultFlag.Success, true)]
        public void Should_Return_Success_Flag_In_Result(BookingResultFlag bookingSuccessFlag, bool isAvailable)
        {
            if (!isAvailable)
                _availableRooms.Clear();
            var result = _processor.BookRoom(_request);
            bookingSuccessFlag.ShouldBe(result.Flag);
        }
        [Theory]
        [InlineData(null, false)]
        [InlineData(1, true)]
        public void Should_Return_BookingId_In_Result(int? roomBookingId, bool isAvailable)
        {
            if (!isAvailable)
                _availableRooms.Clear();
            else
            {
                _roomBookingServiceMock.Setup(x => x.SaveBooking(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(booking =>
                {
                    booking.Id = roomBookingId.Value;
                });
                var result = _processor.BookRoom(_request);
                result.RoomBookingId.ShouldBe(roomBookingId);
            }

        }
    }
}
