using GG_shopping_cart;
using GG_shopping_cart.Controllers;
using GG_shopping_cart.DTO;
using GG_shopping_cart.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace GG_shopping_cart_test
{
	public class CategoryUnitTestController
	{
        private readonly Mock<ICategoryService> categoryService;
        private readonly Mock<ILogger<UsersController>> logger;

        public CategoryUnitTestController()
		{
            logger = new Mock<ILogger<UsersController>>();
			categoryService = new Mock<ICategoryService>();
        }

        [Fact]
        public async Task Get_ReturnsOkResultWithAllCategories()
        {
            //Arrange
            var categoryDtos = new List<CategoryDto>
            {
                new CategoryDto
                {
                    Id = new Guid(),
                    Title = "Food",
                },
                new CategoryDto
                {
                    Id = new Guid(),
                    Title = "Drinks",
                },
            };

            categoryService.Setup(service => service.GetAllCategories())
                                  .ReturnsAsync(categoryDtos);
            var controller = new CategoriesController(categoryService.Object, logger.Object);

            // Act

            var result = await controller.Get();

            // Assert

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okObjectResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(categoryDtos, response.Result);
        }

        [Fact]
        public async Task Get_ReturnsInternalServerErrorOnErrorForAllCategories()
        {
            // Arrange
            categoryService.Setup(service => service.GetAllCategories())
                              .ThrowsAsync(new Exception("An error occurred"));

            var controller = new CategoriesController(categoryService.Object, logger.Object);

            // Act
            var result = await controller.Get();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Get_ReturnsOkResultWithSingleCategory()
        {
            //Arrange
            var id = new Guid();
            var categoryDto = new CategoryDto
            {
                Id = id,
                Title = "Food",
            };


            categoryService.Setup(service => service.GetCategory(id.ToString()))
                                  .ReturnsAsync(categoryDto);
            var controller = new CategoriesController(categoryService.Object, logger.Object);

            // Act

            var result = await controller.Get(id.ToString());

            // Assert

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okObjectResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(categoryDto, response.Result);
        }

        [Fact]
        public async Task Get_ReturnsInternalServerErrorOnErrorForSingleCategory()
        {
            // Arrange
            var id = new Guid();
            categoryService.Setup(service => service.GetCategory(id.ToString()))
                              .ThrowsAsync(new Exception("An error occurred"));

            var controller = new CategoriesController(categoryService.Object, logger.Object);

            // Act
            var result = await controller.Get(id.ToString());

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Post_ReturnsOkResultWithSingleCategory()
        {
            //Arrange
            var id = new Guid();

            var categoryBaseDto = new CategoryBaseDto
            {
                Title = "Drinks",
            };
            var categoryDto = new CategoryDto
            {
                Id = id,
                Title = "Drinks",
            };


            categoryService.Setup(service => service.CreateCategoryAsync(categoryBaseDto))
                                  .ReturnsAsync(categoryDto);
            var controller = new CategoriesController(categoryService.Object, logger.Object);

            // Act

            var result = await controller.Post(categoryBaseDto);

            // Assert

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okObjectResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(categoryDto, response.Result);
        }

        [Fact]
        public async Task Post_ReturnsInternalServerErrorOnErrorForSingleCategory()
        {
            // Arrange
            var categoryBaseDto = new CategoryBaseDto
            {
                Title = "Drinks",
            };
            categoryService.Setup(service => service.CreateCategoryAsync(categoryBaseDto))
                              .ThrowsAsync(new Exception("An error occurred"));

            var controller = new CategoriesController(categoryService.Object, logger.Object);

            // Act
            var result = await controller.Post(categoryBaseDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Put_ReturnsOkResultWithSingleCategory()
        {
            //Arrange
            var id = new Guid();
            var categoryDto = new CategoryDto
            {
                Id = id,
                Title = "Drinks",
            };


            categoryService.Setup(service => service.UpdateCategoryAsync(categoryDto))
                                  .ReturnsAsync(categoryDto);

            var controller = new CategoriesController(categoryService.Object, logger.Object);

            // Act

            var result = await controller.Put(categoryDto);

            // Assert

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okObjectResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(categoryDto, response.Result);
        }

        [Fact]
        public async Task Put_ReturnsInternalServerErrorOnErrorForSingleCategory()
        {
            // Arrange
            var categoryDto = new CategoryDto
            {
                Id = new Guid(),
                Title = "Drinks",
            };
            categoryService.Setup(service => service.UpdateCategoryAsync(categoryDto))
                              .ThrowsAsync(new Exception("An error occurred"));

            var controller = new CategoriesController(categoryService.Object, logger.Object);

            // Act
            var result = await controller.Put(categoryDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsOkResultWithSingleCategory()
        {
            //Arrange
            var id = new Guid();
            categoryService.Setup(service => service.DeleteCategory(id.ToString()))
                                  .ReturnsAsync(true);
            var controller = new CategoriesController(categoryService.Object, logger.Object);

            // Act

            var result = await controller.Delete(id.ToString());

            // Assert

            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDto>(okObjectResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(true, response.Result);
        }

        [Fact]
        public async Task Delete_ReturnsInternalServerErrorOnErrorForSingleCategory()
        {
            // Arrange
            var id = new Guid();
            categoryService.Setup(service => service.DeleteCategory(id.ToString()))
                              .ThrowsAsync(new Exception("An error occurred"));

            var controller = new CategoriesController(categoryService.Object, logger.Object);

            // Act

            var result = await controller.Delete(id.ToString());

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}

