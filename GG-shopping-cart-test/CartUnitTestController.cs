using GG_shopping_cart.Controllers;
using GG_shopping_cart.DTO;
using GG_shopping_cart.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace GG_shopping_cart_test
{
	public class CartUnitTestController
	{
        private readonly Mock<ICartService> cartService;
        private readonly Mock<ILineItemService> lineItemService;
        private readonly Mock<ILogger<UsersController>> logger;
        public CartUnitTestController()
		{
			cartService = new Mock<ICartService>();
            lineItemService = new Mock<ILineItemService>();
            logger = new Mock<ILogger<UsersController>>();
        }

        [Fact]
        public async Task Get_ReturnsOkResultWithCart()
        {
            string clientSession = "session-token";
            var id = new Guid();
            var cartDto = new CartDto
            {
                Id = id,
                SubTotal = 500,
                UserSession = clientSession,
            };
            var lineItems = new List<LineItemDto>
            {
                new LineItemDto
                {
                    Id = new Guid(),
                    CartId = id,
                    Quantity = 2,
                    Product = new ProductDto {
                        Id = new Guid(),
                        Title = "Gummy Bear",
                        Description = "Sweets for kids",
                        Price = 15,
                        ImageUrl = "http//picture1.com",
                        CategoryId = new Guid(),
                    }
                }
            };

            var expectedResult = new CartLineItemsDto
            {
                Id = id,
                SubTotal = 500,
                Items = lineItems
            };



            cartService.Setup(service => service.GetCart(clientSession))
                                  .ReturnsAsync(cartDto);
            lineItemService.Setup(service => service.GetCartLineItems(cartDto.Id.ToString()))
                                .ReturnsAsync(lineItems);

            var controller = new CartsController(cartService.Object, lineItemService.Object, logger.Object);

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Client_App_Session"] = clientSession;
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };


            // Act

            var result = await controller.Get();

            // Assert

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okObjectResult.Value);
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Result);
            Assert.Equal(expectedResult, response.Result);

        }

        [Fact]
        public async Task Get_InvalidSession_ReturnsNotFound()
        {
            // Arrange
            string clientSession = "session-token";

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Client_App_Session"] = clientSession;

            var controller = new CartsController(cartService.Object, lineItemService.Object, logger.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            cartService.Setup(service => service.GetCart("invalid-session-token"))
                            .ReturnsAsync((CartDto)null);

            // Act
            var result = await controller.Get();

            // Assert
            var notFoundResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Get_Exception_ReturnsInternalServerError()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Client_App_Session"] = "valid-session-token";
            var controller = new CartsController(cartService.Object, lineItemService.Object, logger.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            cartService.Setup(service => service.GetCart("valid-session-token"))
                            .ThrowsAsync(new Exception("Test exception"));

            // Act
            var result = await controller.Get();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Post_ReturnsOkResultWithSingleLineCart()
        {
            //Arrange
            var id = new Guid();

            var cartBaseDto = new CartBaseDto
            {
                SubTotal = 500,
                UserSession = "user-session",
            };
            var cartDto = new CartDto
            {
                Id = id,
                SubTotal = 500,
                UserSession = "user-session",
            };
            var expectedResult = new CartLineItemsDto
            {
                Id = id,
                SubTotal = 500,
                Items = new List<LineItemDto>(),
            };


            cartService.Setup(service => service.CreateCartAsync(cartBaseDto))
                                  .ReturnsAsync(cartDto);
            var controller = new CartsController(cartService.Object, lineItemService.Object, logger.Object);

            // Act

            var result = await controller.Post(cartBaseDto);

            // Assert

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okObjectResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(expectedResult, response.Result);
        }

        [Fact]
        public async Task Post_ReturnsInternalServerErrorOnErrorForSingleCart()
        {
            // Arrange
            var cartBaseDto = new CartBaseDto
            {
                SubTotal = 500,
                UserSession = "user-session",
            };
            cartService.Setup(service => service.CreateCartAsync(cartBaseDto))
            .ThrowsAsync(new Exception("An error occurred"));

            var controller = new CartsController(cartService.Object, lineItemService.Object, logger.Object);

            // Act

            var result = await controller.Post(cartBaseDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Post_ReturnsOkResultWithSingleLineItem()
        {
            //Arrange
            var id = new Guid();

            var lineItemBaseDto = new LineItemBaseDto
            {
                CartId = new Guid(),
                Quantity = 2,
                Product = new ProductDto
                {
                    Id = new Guid(),
                    Title = "Gummy Bear",
                    Description = "Sweets for kids",
                    Price = 15,
                    ImageUrl = "http//picture1.com",
                    CategoryId = new Guid(),
                }
            };
            var lineItemDto = new LineItemDto
            {
                Id = id,
                CartId = new Guid(),
                Quantity = 2,
                Product = new ProductDto
                {
                    Id = new Guid(),
                    Title = "Gummy Bear",
                    Description = "Sweets for kids",
                    Price = 15,
                    ImageUrl = "http//picture1.com",
                    CategoryId = new Guid(),
                }
            };


            lineItemService.Setup(service => service.CreateLineItemAsync(lineItemBaseDto))
                                  .ReturnsAsync(lineItemDto);
            var controller = new CartsController(cartService.Object, lineItemService.Object, logger.Object);

            // Act

            var result = await controller.Post(lineItemBaseDto);

            // Assert

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okObjectResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(lineItemDto, response.Result);
        }

        [Fact]
        public async Task Post_ReturnsInternalServerErrorOnErrorForSingleLineItem()
        {
            // Arrange
            var lineItemBaseDto = new LineItemBaseDto
            {
                CartId = new Guid(),
                Quantity = 2,
                Product = new ProductDto
                {
                    Id = new Guid(),
                    Title = "Gummy Bear",
                    Description = "Sweets for kids",
                    Price = 15,
                    ImageUrl = "http//picture1.com",
                    CategoryId = new Guid(),
                }
            };
            lineItemService.Setup(service => service.CreateLineItemAsync(lineItemBaseDto))
            .ThrowsAsync(new Exception("An error occurred"));

            var controller = new CartsController(cartService.Object, lineItemService.Object, logger.Object);

            // Act

            var result = await controller.Post(lineItemBaseDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsOkResultWithSingleLineItem()
        {
            //Arrange
            var id = new Guid();
            lineItemService.Setup(service => service.DeleteLineItem(id.ToString()))
                                  .ReturnsAsync(true);
            var controller = new CartsController(cartService.Object, lineItemService.Object, logger.Object);

            // Act

            var result = await controller.DeleteLineItem(id.ToString());

            // Assert

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okObjectResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(true, response.Result);
        }

        [Fact]
        public async Task Delete_ReturnsInternalServerErrorOnErrorForSingleLineItem()
        {
            // Arrange
            var id = new Guid();
            lineItemService.Setup(service => service.DeleteLineItem(id.ToString()))
                              .ThrowsAsync(new Exception("An error occurred"));

            var controller = new CartsController(cartService.Object, lineItemService.Object, logger.Object);

            // Act

            var result = await controller.DeleteLineItem(id.ToString());

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsOkResultWithSingleCart()
        {
            //Arrange
            var clientSession = "some_session";
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Client_App_Session"] = clientSession;

            cartService.Setup(service => service.DeleteCart(clientSession))
                                  .ReturnsAsync(true);
            var controller = new CartsController(cartService.Object, lineItemService.Object, logger.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            // Act

            var result = await controller.DeleteCart();

            // Assert

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okObjectResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(true, response.Result);
        }

        [Fact]
        public async Task Delete_ReturnsInternalServerErrorOnErrorForSingleCart()
        {
            // Arrange
            var clientSession = "some_session";
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Client_App_Session"] = clientSession;

            cartService.Setup(service => service.DeleteCart(clientSession))
                              .ThrowsAsync(new Exception("An error occurred"));

            var controller = new CartsController(cartService.Object, lineItemService.Object, logger.Object);
            controller.ControllerContext = new ControllerContext { HttpContext = httpContext };

            // Act

            var result = await controller.DeleteCart();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}

