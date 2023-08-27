
using Amazon.S3;
using GG_shopping_cart.Controllers;
using GG_shopping_cart.DTO;
using GG_shopping_cart.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace GG_shopping_cart_test;

public class ProductUnitTestController
{
    private readonly Mock<IProductService> productService;
    private readonly Mock<IConfiguration> configuration;
    private readonly Mock<ILogger<UsersController>> logger;
    private readonly Mock<IAmazonS3> s3Client;
    private readonly string bucketName;

    public ProductUnitTestController()
    {
        productService = new Mock<IProductService>();
        configuration = new Mock<IConfiguration>();
        s3Client = new Mock<IAmazonS3>();
        bucketName = "mock_bucket";
        logger = new Mock<ILogger<UsersController>>();
    }

    [Fact]
    public async Task Get_ReturnsOkResultWithAllProducts()
    {
        //Arrange
        var productDtos = new List<ProductDto>
        {
            new ProductDto
            {
                Id = new Guid(),
                Title = "Gummy Bear",
                Description = "Sweets for kids",
                Price = 15,
                ImageUrl = "http//picture1.com",
                CategoryId = new Guid(),
            },
            new ProductDto
            {
                Id = new Guid(),
                Title = "Lays",
                Description = "Chips",
                Price = 24,
                ImageUrl = "http//picture2.com",
                CategoryId = new Guid(),
            },
        };

        productService.Setup(service => service.GetAllProducts())
                              .ReturnsAsync(productDtos);
        var controller = new ProductsController(
            productService.Object,
            s3Client.Object,
            configuration.Object,
            logger.Object
        );

        // Act

        var result = await controller.Get();

        // Assert

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseDto>(okObjectResult.Value);
        Assert.True(response.IsSuccess);
        Assert.Equal(productDtos, response.Result);
    }

    [Fact]
    public async Task Get_ReturnsInternalServerErrorOnErrorForAllProducts()
    {
        // Arrange
        productService.Setup(service => service.GetAllProducts())
                          .ThrowsAsync(new Exception("An error occurred"));

        var controller = new ProductsController(
           productService.Object,
           s3Client.Object,
           configuration.Object,
           logger.Object
       );

        // Act
        var result = await controller.Get();

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task Get_ReturnsOkResultWithSingleProduct()
    {
        //Arrange
        var id = new Guid();
        var productDto = new ProductDto
        {
            Id = id,
            Title = "Gummy Bear",
            Description = "Sweets for kids",
            Price = 15,
            ImageUrl = "http//picture1.com",
            CategoryId = new Guid(),
        };
           

        productService.Setup(service => service.GetProduct(id.ToString()))
                              .ReturnsAsync(productDto);
        var controller = new ProductsController(
            productService.Object,
            s3Client.Object,
            configuration.Object,
            logger.Object
        );

        // Act

        var result = await controller.Get(id.ToString());

        // Assert

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseDto>(okObjectResult.Value);
        Assert.True(response.IsSuccess);
        Assert.Equal(productDto, response.Result);
    }

    [Fact]
    public async Task Get_ReturnsInternalServerErrorOnErrorForSingleProduct()
    {
        // Arrange
        var id = new Guid();
        productService.Setup(service => service.GetProduct(id.ToString()))
                          .ThrowsAsync(new Exception("An error occurred"));

        var controller = new ProductsController(
           productService.Object,
           s3Client.Object,
           configuration.Object,
           logger.Object
       );

        // Act
        var result = await controller.Get(id.ToString());

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task Post_ReturnsOkResultWithSingleProduct()
    {
        //Arrange
        var id = new Guid();
        var categoryId = new Guid();

        var productBaseDto = new ProductBaseDto
        {
            Title = "Gummy Bear",
            Description = "Sweets for kids",
            Price = 15,
            ImageUrl = "http//picture1.com",
            CategoryId = categoryId,
        };
        var productDto = new ProductDto
        {
            Id = id,
            Title = "Gummy Bear",
            Description = "Sweets for kids",
            Price = 15,
            ImageUrl = "http//picture1.com",
            CategoryId = categoryId,
        };


        productService.Setup(service => service.CreateProductAsync(productBaseDto))
                              .ReturnsAsync(productDto);
        var controller = new ProductsController(
            productService.Object,
            s3Client.Object,
            configuration.Object,
            logger.Object
        );

        // Act

        var result = await controller.Post(productBaseDto);

        // Assert

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseDto>(okObjectResult.Value);
        Assert.True(response.IsSuccess);
        Assert.Equal(productDto, response.Result);
    }

    [Fact]
    public async Task Post_ReturnsInternalServerErrorOnErrorForSingleProduct()
    {
        // Arrange
        var productBaseDto = new ProductBaseDto
        {
            Title = "Gummy Bear",
            Description = "Sweets for kids",
            Price = 15,
            ImageUrl = "http//picture1.com",
            CategoryId = new Guid(),
        };
        productService.Setup(service => service.CreateProductAsync(productBaseDto))
                          .ThrowsAsync(new Exception("An error occurred"));

        var controller = new ProductsController(
           productService.Object,
           s3Client.Object,
           configuration.Object,
           logger.Object
       );

        // Act
        var result = await controller.Post(productBaseDto);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task Put_ReturnsOkResultWithSingleProduct()
    {
        //Arrange

        var productDto = new ProductDto
        {
            Id = new Guid(),
            Title = "Gummy Bear",
            Description = "Sweets for kids",
            Price = 15,
            ImageUrl = "http//picture1.com",
            CategoryId = new Guid(),
        };


        productService.Setup(service => service.UpdateProductAsync(productDto))
                              .ReturnsAsync(productDto);
        var controller = new ProductsController(
            productService.Object,
            s3Client.Object,
            configuration.Object,
            logger.Object
        );

        // Act

        var result = await controller.Put(productDto);

        // Assert

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseDto>(okObjectResult.Value);
        Assert.True(response.IsSuccess);
        Assert.Equal(productDto, response.Result);
    }

    [Fact]
    public async Task Put_ReturnsInternalServerErrorOnErrorForSingleProduct()
    {
        // Arrange
        var productDto = new ProductDto
        {
            Id = new Guid(),
            Title = "Gummy Bear",
            Description = "Sweets for kids",
            Price = 15,
            ImageUrl = "http//picture1.com",
            CategoryId = new Guid(),
        };
        productService.Setup(service => service.UpdateProductAsync(productDto))
                          .ThrowsAsync(new Exception("An error occurred"));

        var controller = new ProductsController(
           productService.Object,
           s3Client.Object,
           configuration.Object,
           logger.Object
       );

        // Act
        var result = await controller.Put(productDto);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsOkResultWithSingleProduct()
    {
        //Arrange
        var id = new Guid();
        productService.Setup(service => service.DeleteProduct(id.ToString()))
                              .ReturnsAsync(true);
        var controller = new ProductsController(
            productService.Object,
            s3Client.Object,
            configuration.Object,
            logger.Object
        );

        // Act

        var result = await controller.Delete(id.ToString());

        // Assert

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<ResponseDto>(okObjectResult.Value);
        Assert.True(response.IsSuccess);
        Assert.Equal(true, response.Result);
    }

    [Fact]
    public async Task Delete_ReturnsInternalServerErrorOnErrorForSingleProduct()
    {
        // Arrange
        var id = new Guid();
        productService.Setup(service => service.DeleteProduct(id.ToString()))
                          .ThrowsAsync(new Exception("An error occurred"));

        var controller = new ProductsController(
           productService.Object,
           s3Client.Object,
           configuration.Object,
           logger.Object
       );

        // Act
        var result = await controller.Delete(id.ToString());

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task UploadImage_ValidFile_ReturnsOkResult()
    {
        // Arrange
        
        configuration.Setup(c => c.GetSection("AppSettings:UploadsLocation").Value).Returns("Uploads");

        var controller = new ProductsController(
           productService.Object,
           s3Client.Object,
           configuration.Object,
           logger.Object
       );

        var formFile = new FormFile(new MemoryStream(new byte[] { 0x20, 0x20 }), 0, 2, "imageFile", "test.jpg");

        // Act
        var result = await controller.UploadImage(formFile) as ObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task UploadImage_InvalidFile_ReturnsBadRequest()
    {
        // Arrange
        configuration.Setup(c => c.GetSection("AppSettings:UploadsLocation").Value).Returns("Uploads");

        var controller = new ProductsController(
           productService.Object,
           s3Client.Object,
           configuration.Object,
           logger.Object
       );

        var formFile = new FormFile(new MemoryStream(new byte[] { }), 0, 0, "imageFile", "test.jpg");

        // Act
        var result = await controller.UploadImage(formFile) as BadRequestObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequestObjectResult>(result);
    }

}
