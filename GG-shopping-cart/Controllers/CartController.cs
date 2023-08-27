using Microsoft.AspNetCore.Mvc;
using GG_shopping_cart.DTO;
using GG_shopping_cart.Services;
using GG_shopping_cart.Entities;
using Microsoft.AspNetCore.Authorization;

namespace GG_shopping_cart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CartsController: ControllerBase
	{
        private ICartService _cartService;
        private ILineItemService _lineItemService;
        private ResponseDto _response;
        private readonly ILogger<UsersController> _logger;

        public CartsController(
            ICartService cartService,
            ILineItemService lineItemService,
            ILogger<UsersController> logger
        )
        {
            _cartService = cartService;
            _lineItemService = lineItemService;
            _response = new ResponseDto();
            _logger = logger;
        }

        // GET: api/<CartController>
        [HttpGet]
        public async Task<object> Get() // tobe removed
        {
            _logger.LogInformation("Cart: Request initiated");
            try
            {
                HttpContext context = HttpContext;
                if (context.Request.Headers.TryGetValue("Client_App_Session", out var clientSession))
                {
                    var cartDto = await _cartService.GetCart(clientSession);

                    if (cartDto != null)
                    {
                        List<LineItemDto> lineItems = await _lineItemService.GetCartLineItems(cartDto.Id.ToString());
                        _response.Result = new CartLineItemsDto
                        {
                            Id = cartDto.Id,
                            SubTotal = cartDto.SubTotal,
                            Items = lineItems,
                        };
                        return Ok(_response);
                    }
                    else
                    {
                        _logger.LogError("Cart: Cart not found");
                        _response.IsSuccess = false;
                        _response.Result = null;

                        return StatusCode(404, _response);
                    }

                }
                _logger.LogError("Cart: Missing Header");
                _response.IsSuccess = false;
                _response.Result = null;

                return StatusCode(404, _response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Cart: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };

                return StatusCode(500, _response);
            }
        }

        //POST: api/<CartController>
        [HttpPost("cart")]
        public async Task<object> Post([FromBody] CartBaseDto cartDto)
        {
            _logger.LogInformation("Cart: Request initiated");

            if (!ModelState.IsValid)
            {
                _logger.LogError("Cart: Invalid data", cartDto);
                return BadRequest(ModelState);
            }

            try
            {
                var lineItems = new List<LineItemDto>();

                CartDto cart = await _cartService.CreateCartAsync(cartDto);


                _response.Result = new CartLineItemsDto
                {
                    Id = cart.Id,
                    SubTotal = cart.SubTotal,
                    Items = lineItems,
                };

                return Ok(_response);

            }
            catch (Exception ex)
            {
                _logger.LogError("Cart: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };

                return StatusCode(500, _response);
            }
        }

        // POST: api/<CartsController>
        [HttpPost("lineitem")]
        public async Task<object> Post([FromBody] LineItemBaseDto lineItemDto)
        {
            _logger.LogInformation("Cart: Request initiated");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Cart: Invalid data", lineItemDto);
                return BadRequest(ModelState);
            }

            try
            {
                LineItemDto lineItem = await _lineItemService.CreateLineItemAsync(lineItemDto);
                _response.Result = lineItem;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Cart: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };

                return StatusCode(500, _response);
            }
        }

        // PUT: api/<CartController>
        [HttpPut("lineitem")]
        public async Task<object> Put([FromBody] LineItemDto lineItemDto)
        {
            _logger.LogInformation("Cart: Request initiated");
            if (!ModelState.IsValid)
            {
                _logger.LogError("Cart: Invalid data", lineItemDto);
                return BadRequest(ModelState);
            }

            try
            {
                LineItemDto lineItem = await _lineItemService.UpdateLineItemAsync(lineItemDto);
                _response.Result = lineItem;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Cart: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };

                return StatusCode(500, _response);
            }
        }

        // DELETE: api/<CartController>/2
        [HttpDelete("cart")]
        public async Task<object> DeleteCart()
        {
            _logger.LogInformation("Cart: Request initiated");
            try
            {
                HttpContext context = HttpContext;
                if (context.Request.Headers.TryGetValue("Client_App_Session", out var clientSession))
                {
                    _logger.LogInformation("Cart: Deleted", clientSession);
                    bool status = await _cartService.DeleteCart(clientSession);
                    _response.Result = status;

                    return Ok(_response);

                }

                _logger.LogError("Cart: Missing Header");
                _response.IsSuccess = false;
                _response.Result = null;

                return StatusCode(404, _response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Cart: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };

                return StatusCode(500, _response);
            }
        }

        // DELETE: api/<CartController>/2
        [HttpDelete("lineitem/{id}")]
        public async Task<object> DeleteLineItem(string id)
        {
            _logger.LogInformation("Lineitem: Request initiated");
            try
            {
                _logger.LogInformation("Lineitem: Deleted", id);
                bool status = await _lineItemService.DeleteLineItem(id);
                _response.Result = status;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _logger.LogError("Lineitem: Server error", ex);
                _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };

                return StatusCode(500, _response);
            }
        }

    }
}

