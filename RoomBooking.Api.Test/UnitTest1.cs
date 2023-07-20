using Microsoft.Extensions.Logging;
using Moq;
using RoomBooking.Api.Controllers;
using Shouldly;
using System;
using System.Linq;
using Xunit;

namespace RoomBooking.Api.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Should_Return_Forcast_Results()
        {
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMock.Object);

            var result = controller.Get();

            result.Count().ShouldBeGreaterThan(1);
            result.ShouldNotBeNull();
        }
    }
}
