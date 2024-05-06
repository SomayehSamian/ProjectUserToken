using Microsoft.Extensions.Configuration;
using Project.Core.Contract.Repository;
using Project.Core.Implementation;
using Moq;

namespace TestProject
{
    public class UserServiceTest
    {

        [Fact]
        public void GenerateReturnsToken()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(x => x["Jwt:Secret"]).Returns("ldpspc4d5e6f7g8h9i0j1k2l3m4n5o6p");

            // Arrange
            var userService = new UserService(userRepositoryMock.Object, configurationMock.Object);
            string username = "Ssh";

            // Act
            string token = userService.GenerateJwtToken(username);

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);

        }
    }
}