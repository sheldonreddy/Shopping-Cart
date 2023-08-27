using Microsoft.AspNetCore.Mvc;
using GG_shopping_cart.DTO;
using GG_shopping_cart.Services;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using GG_shopping_cart.Helpers;
using Microsoft.Extensions.Configuration;

namespace GG_shopping_cart.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
    [Authorize]

    public class ProductsController: ControllerBase
	{
		private readonly IProductService _productService;
		private readonly ResponseDto _response;
        private readonly IAmazonS3 _s3Client;
        private readonly string _uploadLocation;
        private readonly ILogger<UsersController> _logger;


        public ProductsController(
            IProductService productService,
            IAmazonS3 s3Client,
            IConfiguration configuration,
            ILogger<UsersController> logger
        )
		{
            _logger = logger;
            _productService = productService;
			_response = new ResponseDto();
            _s3Client = s3Client;
            _uploadLocation = ConfigurationHelpers.GetConfigurationValue(configuration, "UploadsDirectory", "");
        }

		// GET: api/<ProductsController>
		[HttpGet]
        [SwaggerOperation(
            Summary = "Fetch all the products",
            Description = "Fetch products for the authenticated user."
        )]
        public async Task<object> Get()
		{
            _logger.LogInformation("Product: Request initiated");
            try
			{
				IEnumerable<ProductDto> productDto = await _productService.GetAllProducts();
				_response.Result = productDto;

                return Ok(_response);
            } catch (Exception ex)
			{
                _logger.LogError("Product: Request failed", ex);
                _response.IsSuccess = false;
				_response.Errors = new List<string> { ex.Message };
                return StatusCode(500, _response);
            }
		}

        // GET: api/<ProductsController>/2
        [HttpGet("{id}")]
		public async Task<object> Get(string id)
		{
            _logger.LogInformation("Product: Request initiated");
            try
			{
				var product = await _productService.GetProduct(id);
				if (product != null)
				{
					_response.Result = product;
                    return Ok(_response);
				}

                _logger.LogError("Product: Product not found", id);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { "Product not found" };
                return StatusCode(404, _response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Product: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };
                return StatusCode(500, _response);
            }
        }


		// POST: api/<ProductsController>
		[HttpPost]
		public async Task<object> Post([FromBody] ProductBaseDto product)
		{
            _logger.LogInformation("Product: Request initiated");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Product: Invalid data", product);
                return BadRequest(ModelState);
            }

            try
			{
				ProductDto productDto = await _productService.CreateProductAsync(product);
				_response.Result = productDto;

                return Ok(_response);
			}
            catch (Exception ex)
            {
                _logger.LogError("Product: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };

                return StatusCode(500, _response);
            }
        }

        // PUT: api/<ProductsController>
        [HttpPut]
        public async Task<object> Put([FromBody] ProductDto product)
        {
            _logger.LogInformation("Product: Request initiated");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Product: Invalid data", product);
                return BadRequest(ModelState);
            }

            try
            {
                ProductDto productDto = await _productService.UpdateProductAsync(product);
                _response.Result = productDto;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Product: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };

                return StatusCode(500, _response);
            }
        }

        // DELETE: api/<ProductsController>
        [HttpDelete("{id}")]
        public async Task<object> Delete(string id)
        {
            _logger.LogInformation("Product: Request initiated");
            try
            {
                bool status = await _productService.DeleteProduct(id);
                _response.Result = status;

                _logger.LogInformation("Product: Deleted", id);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Product: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };

                return StatusCode(500, _response);
            }
        }

        [HttpPost("upload")]
        public async Task<object> UploadImage(IFormFile imageFile)
        {
            _logger.LogInformation("Product: Request initiated");
            try
            {
                if (imageFile == null || imageFile.Length <= 0)
                {
                    _logger.LogError("Product: Invalid data", imageFile);
                    return BadRequest("Image file is required.");
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                string filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), _uploadLocation), uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                string imageUrl = "/Uploads/" + uniqueFileName;

                return Ok(new { ImageUrl = imageUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError("Product: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };

                return StatusCode(500, _response);
            }

        }

    }
}

