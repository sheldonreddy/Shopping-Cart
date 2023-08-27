using GG_shopping_cart.Controllers;
using GG_shopping_cart.DTO;
using GG_shopping_cart.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Logging;

namespace GG_shopping_cart_test
{
    public class UserUnitTestController: BaseTest
    {
        private readonly Mock<UserManager<ApplicationUser>> userManager;
        private readonly Mock<ILogger<UsersController>> logger;

        public UserUnitTestController()
        {
            userManager = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null
            );
            logger = new Mock<ILogger<UsersController>>();
        }

        [Fact]
        public async Task Register_ValidRegistration_ReturnsOkResult()
        {
            // Arrange
            var registrationDto = new RegistrationDto
            {
                Email = "test@example.com",
                Firstname = "John",
                Lastname = "Doe",
                Password = "password"
            };

            userManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var controller = new UsersController(userManager.Object, configuration, logger.Object);
            // Act
            var result = await controller.Register(registrationDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okResult.Value);

            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Result);
        }

        [Fact]
        public async Task Register_InvalidRegistration_ReturnsBadRequest()
        {
            // Arrange
            var registrationDto = new RegistrationDto();

            var controller = new UsersController(userManager.Object, configuration, logger.Object);
            controller.ModelState.AddModelError("Email", "Email is required");

            // Act
            var result = await controller.Register(registrationDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<SerializableError>(badRequestResult.Value);

            Assert.False(controller.ModelState.IsValid);
            Assert.True(response.ContainsKey("Email"));
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResultWithToken()
        {
            // Arrange
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Firstname = "John",
                Lastname = "Doe"
            };

            userManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);
            userManager.Setup(um => um.CheckPasswordAsync(user, It.IsAny<string>())).ReturnsAsync(true);

            var controller = new UsersController(userManager.Object, configuration, logger.Object);

            // Act
            var result = await controller.Login(new UserLogin { Email = user.Email, Password = "password" });

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okResult.Value);

            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Result);

        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsNotFoundResult()
        {
            // Arrange

            userManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
            var controller = new UsersController(userManager.Object, configuration, logger.Object);

            // Act
            var result = await controller.Login(new UserLogin { Email = "test@example.com", Password = "invalidPassword" });

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, statusCodeResult.StatusCode);
            
        }

    }
}

