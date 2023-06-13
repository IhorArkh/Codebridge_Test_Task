using DogsAPI.Controllers;
using DogsAPI.DB;
using DogsAPI.DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DogsTests
{
    public class PingTest
    {
        [Fact]
        public void Ping_ReturnsOkObjectResultAndCorrectMessage()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DogsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var dbContext = new DogsContext(dbContextOptions))
            {
                var controller = new DogsController(dbContext);

                // Act
                var result = controller.Ping();

                // Assert
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal("Dogs house service. Version 1.0.1", okResult.Value);
            }
        }
    }
}