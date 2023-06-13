using DogsAPI.Controllers;
using DogsAPI.DB;
using DogsAPI.DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DogsTests
{
    public class CreateDogTest
    {
        [Fact]
        public async Task CreateDog_WithValidData_ReturnsOkResultAndMessage()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DogsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var dbContext = new DogsContext(dbContextOptions))
            {
                var controller = new DogsController(dbContext);
                var dog = new Dog
                {
                    Name = "Test",
                    Color = "Test",
                    TailLength = 10,
                    Weight = 20
                };

                // Act
                var result = await controller.CreateDog(dog);

                // Assert
                Assert.IsType<OkObjectResult>(result);
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.Equal(okResult.Value, "Dog created successfully.");
            }
        }

        [Fact]
        public async Task CreateDog_WithNegativeTail_ReturnsMessageAndStatusCode()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DogsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var dbContext = new DogsContext(dbContextOptions))
            {
                var controller = new DogsController(dbContext);
                var dog = new Dog
                {
                    Name = "Test",
                    Color = "Test",
                    TailLength = -10,
                    Weight = 20
                };

                // Act
                var result = await controller.CreateDog(dog);

                // Assert
                var badResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(badResult.Value, "Tail length cannot be a negative number.");
                Assert.True(badResult.StatusCode == 400);
            }
        }

        [Fact]
        public async Task CreateDog_WithNegativeWeight_ReturnsMessageAndStatusCode()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DogsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var dbContext = new DogsContext(dbContextOptions))
            {
                var controller = new DogsController(dbContext);
                var dog = new Dog
                {
                    Name = "Test",
                    Color = "Test",
                    TailLength = 10,
                    Weight = -20
                };

                // Act
                var result = await controller.CreateDog(dog);

                // Assert
                var badResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(badResult.Value, "Weight cannot be a negative number.");
                Assert.True(badResult.StatusCode == 400);
            }
        }

        [Fact]
        public async Task CreateDog_WithInvalidModel_ReturnsMessageAndStatusCode()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DogsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var dbContext = new DogsContext(dbContextOptions))
            {
                var controller = new DogsController(dbContext);
                var dog = new Dog
                {
                    Name = "Test",
                    Color = "Test",
                    TailLength = 10,
                };

                // Act
                var result = await controller.CreateDog(dog);

                // Assert
                var badResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.Equal(badResult.Value, "Invalid dog data.");
                Assert.True(badResult.StatusCode == 400);
            }
        }

        [Fact]
        public async Task CreateDog_WithExistingName_ReturnsMessageAndStatusCode()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DogsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            using (var dbContext = new DogsContext(dbContextOptions))
            {
                var controller = new DogsController(dbContext);
                var dog = new Dog
                {
                    Name = "Jessy",
                    Color = "Test",
                    TailLength = 10,
                    Weight = 20
                };

                // Act
                _ = await controller.CreateDog(dog);
                var result = await controller.CreateDog(dog);

                // Assert
                var conflictResult = Assert.IsType<ConflictObjectResult>(result);
                Assert.Equal(conflictResult.Value, "A dog with the same name already exists.");
                Assert.True(conflictResult.StatusCode == 409);
            }
        }
    }
}