using Microsoft.AspNetCore.Mvc;
using GG_shopping_cart.DTO;
using GG_shopping_cart.Services;
using Microsoft.AspNetCore.Authorization;

namespace GG_shopping_cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService;
        private readonly ILogger<UsersController> _logger;
        private ResponseDto _response;

        public CategoriesController(ICategoryService categoryService, ILogger<UsersController> logger)
        {
            _categoryService = categoryService;
            _response = new ResponseDto();
            _logger = logger;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        public async Task<object> Get()
        {
            _logger.LogInformation("Category: Request initiated");
            try
            {
                IEnumerable<CategoryDto> categoryDto = await _categoryService.GetAllCategories();
                _response.Result = categoryDto;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Category: Request failed", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };

                return StatusCode(500, _response);
            }
        }

        // GET: api/<CategoriesController>/2
        [HttpGet("{id}")]
        public async Task<object> Get(string id)
        {
            _logger.LogInformation("Category: Request initiated");
            try
            {
                var category = await _categoryService.GetCategory(id);
                if (category != null)
                {
                    _response.Result = category;
                    return Ok(_response);
                }
                _logger.LogError("Categor: Product not found", id);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { "Product not found" };
                return StatusCode(404, _response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Category: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };

                return StatusCode(500, _response);
            }
        }


        // POST: api/<CategoriesController>
        [HttpPost]
        public async Task<object> Post([FromBody] CategoryBaseDto category)
        {
            _logger.LogInformation("Category: Request initiated");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Category: Invalid data", category);
                return BadRequest(ModelState);
            }

            try
            {
                CategoryDto categoryDto = await _categoryService.CreateCategoryAsync(category);
                _response.Result = categoryDto;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Category: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };

                return StatusCode(500, _response);
            }
        }

        // PUT: api/<CategoriesController>/2
        [HttpPut]
        public async Task<object> Put([FromBody] CategoryDto category)
        {
            _logger.LogInformation("Category: Request initiated");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Category: Invalid data", category);
                return BadRequest(ModelState);
            }

            try
            {
                CategoryDto categoryDto = await _categoryService.UpdateCategoryAsync(category);
                _response.Result = categoryDto;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Category: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };

                return StatusCode(500, _response);
            }
        }

        // DELETE: api/<CategoriesController>
        [HttpDelete("{id}")]
        public async Task<object> Delete(string id)
        {
            _logger.LogInformation("Category: Request initiated");
            try
            {
                bool status = await _categoryService.DeleteCategory(id);
                _response.Result = status;

                _logger.LogInformation("Category: Deleted", id);

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Category: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };

                return StatusCode(500, _response);
            }
        }

    }
}
