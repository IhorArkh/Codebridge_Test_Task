using DogsAPI.Controllers;
using DogsAPI.DB;
using DogsAPI.DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DogsTests
{
    public class GetDogsTest
    {
        [Fact]
        public void GetDogs_ReturnsCorrectListOfDogs()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DogsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase1")
                .Options;

            using (var dbContext = new DogsContext(dbContextOptions))
            {
                var controller = new DogsController(dbContext);

                var expectedDogs = new List<Dog>
                {
                    new Dog { Name = "Dog 1", Color = "Brown", TailLength = 10, Weight = 20 },
                    new Dog { Name = "Dog 2", Color = "Black", TailLength = 15, Weight = 25 },
                    new Dog { Name = "Dog 3", Color = "White", TailLength = 12, Weight = 18 }
                };

                // Act
                foreach (var dog in expectedDogs)
                {
                    controller.CreateDog(dog);
                }

                var result = controller.GetDogs(null, null, 0, 0);
                var okResult = result as OkObjectResult;
                var actualDogs = (okResult?.Value as IQueryable<Dog>).ToList();

                // Assert
                Assert.Equal(expectedDogs, actualDogs);
                Assert.True(okResult.StatusCode == 200);
            }
        }

        [Fact]
        public void GetDogs_WithSortingReturnsCorrectListOfDogs()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DogsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase2")
                .Options;

            using (var dbContext = new DogsContext(dbContextOptions))
            {
                var controller = new DogsController(dbContext);

                var dogs = new List<Dog>
                {
                    new Dog { Name = "Dog 1", Color = "Brown", TailLength = 10, Weight = 20 },
                    new Dog { Name = "Dog 2", Color = "Black", TailLength = 15, Weight = 25 },
                    new Dog { Name = "Dog 3", Color = "White", TailLength = 12, Weight = 18 }
                };

                var expectedDogs = new List<Dog>
                {
                    new Dog { Name = "Dog 2", Color = "Black", TailLength = 15, Weight = 25 },
                    new Dog { Name = "Dog 1", Color = "Brown", TailLength = 10, Weight = 20 },
                    new Dog { Name = "Dog 3", Color = "White", TailLength = 12, Weight = 18 }
                    
                };

                // Act
                foreach (var dog in dogs)
                {
                    controller.CreateDog(dog);
                }

                //var result = controller.GetDogs("weight", "desc", 0, 0);
                var result = controller.GetDogs("weight", "desc", 0, 0);
                var okResult = result as OkObjectResult;
                var actualDogs = (okResult?.Value as IQueryable<Dog>).ToList();

                // Assert
                Assert.True(okResult.StatusCode == 200);

                for (int i = 0; i < expectedDogs.Count; i++)
                {
                    Assert.Equal(expectedDogs[i].Name, actualDogs[i].Name);
                }
            }
        }

        [Fact]
        public void GetDogs_WithPagination_ReturnsCorrectListOfDogs()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DogsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase3")
                .Options;

            using (var dbContext = new DogsContext(dbContextOptions))
            {
                var controller = new DogsController(dbContext);

                var dogs = new List<Dog>
                {
                    new Dog { Name = "Dog 1", Color = "Brown", TailLength = 10, Weight = 20 },
                    new Dog { Name = "Dog 2", Color = "Black", TailLength = 15, Weight = 25 },
                    new Dog { Name = "Dog 3", Color = "White", TailLength = 12, Weight = 18 }
                };

                foreach (var dog in dogs)
                {
                    dbContext.Dogs.Add(dog);
                }
                dbContext.SaveChanges();

                var expectedDogsPage1 = new List<Dog>
                {
                    new Dog { Name = "Dog 1", Color = "Brown", TailLength = 10, Weight = 20 },
                    new Dog { Name = "Dog 2", Color = "Black", TailLength = 15, Weight = 25 },
                };

                var expectedDogsPage2 = new List<Dog>
                {
                    new Dog { Name = "Dog 3", Color = "White", TailLength = 12, Weight = 18 }
                };

                // Act
                var resultPage1 = controller.GetDogs(null, null, 1, 2);
                var okResultPage1 = Assert.IsType<OkObjectResult>(resultPage1);
                var actualDogsPage1 = Assert.IsAssignableFrom<List<Dog>>(okResultPage1.Value);

                var resultPage2 = controller.GetDogs(null, null, 2, 2);
                var okResultPage2 = Assert.IsType<OkObjectResult>(resultPage2);
                var actualDogsPage2 = Assert.IsAssignableFrom<List<Dog>>(okResultPage2.Value);

                // Assert
                Assert.True(okResultPage1.StatusCode == 200 && okResultPage2.StatusCode == 200);

                for (int i = 0; i < expectedDogsPage1.Count; i++)
                {
                    Assert.Equal(expectedDogsPage1[i].Name, actualDogsPage1[i].Name);
                }

                for (int i = 0; i < expectedDogsPage2.Count; i++)
                {
                    Assert.Equal(expectedDogsPage2[i].Name, actualDogsPage2[i].Name);
                }
            }
        }

        [Fact]
        public void GetDogs_WithPaginationAndSorting_ReturnsCorrectListOfDogs()
        {
            // Arrange
            var dbContextOptions = new DbContextOptionsBuilder<DogsContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase4")
                .Options;

            using (var dbContext = new DogsContext(dbContextOptions))
            {
                var controller = new DogsController(dbContext);

                var dogs = new List<Dog>
                {
                    new Dog { Name = "Dog 1", Color = "Brown", TailLength = 10, Weight = 20 },
                    new Dog { Name = "Dog 2", Color = "Black", TailLength = 15, Weight = 25 },
                    new Dog { Name = "Dog 3", Color = "White", TailLength = 12, Weight = 18 }
                };

                foreach (var dog in dogs)
                {
                    dbContext.Dogs.Add(dog);
                }
                dbContext.SaveChanges();

                var expectedDogsPage1 = new List<Dog>
                {
                    new Dog { Name = "Dog 2", Color = "Black", TailLength = 15, Weight = 25 },
                    new Dog { Name = "Dog 1", Color = "Brown", TailLength = 10, Weight = 20 }
                };

                var expectedDogsPage2 = new List<Dog>
                {
                    new Dog { Name = "Dog 3", Color = "White", TailLength = 12, Weight = 18 }
                };

                // Act
                var resultPage1 = controller.GetDogs("weight", "desc", 1, 2);
                var okResultPage1 = Assert.IsType<OkObjectResult>(resultPage1);
                var actualDogsPage1 = Assert.IsAssignableFrom<List<Dog>>(okResultPage1.Value);

                var resultPage2 = controller.GetDogs("weight", "desc", 2, 2);
                var okResultPage2 = Assert.IsType<OkObjectResult>(resultPage2);
                var actualDogsPage2 = Assert.IsAssignableFrom<List<Dog>>(okResultPage2.Value);

                // Assert
                Assert.True(okResultPage1.StatusCode == 200 && okResultPage2.StatusCode == 200);

                for (int i = 0; i < expectedDogsPage1.Count; i++)
                {
                    Assert.Equal(expectedDogsPage1[i].Name, actualDogsPage1[i].Name);
                }

                for (int i = 0; i < expectedDogsPage2.Count; i++)
                {
                    Assert.Equal(expectedDogsPage2[i].Name, actualDogsPage2[i].Name);
                }
            }
        }
    }
}
