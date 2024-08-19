using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Models;
using SewingFactory.Models.DTO;
using SewingFactory.Services.Interface;
using SewingFactory.Services.Service;

namespace SewingFactory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ITokenService _tokenService;

        public OrdersController(IOrderService orderService, ITokenService tokenService)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        // GET: api/Orders
        // Get all orders
        // Only Cashier or Order Manager can use
        [Authorize(Policy = "Cashier-Order")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetAllOrders(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                IEnumerable<GetOrderDTO> orderList = await _orderService.GetAllOrderDTOList(pageNumber, pageSize);
                return Ok(orderList);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Orders/{id}
        // Get 1 order by ID
        // Only Cashier or Order Manager can use
        [Authorize(Policy = "Cashier-Order")]
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetOrderDTO>> GetOrder(Guid id)
        {
            try
            {
                var order = await _orderService.GetOrderDTO(id);
                return Ok(order);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }

        // GET: api/Orders/search
        // Search order list by a specific filter
        // Only Cashier or Order Manager can use
        [Authorize(Policy = "Cashier-Order")]
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> SearchOrderList(int pageNumber = 1, int pageSize = 5, string? firstInputValue = null, string? secondInputValue = null, string filter = "status")
        {
            try
            {
                return Ok(await _orderService.searchOrderDTOList(pageNumber, pageSize, firstInputValue, secondInputValue, filter));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // POST: api/Orders
        // Add an order to database
        // Only Cashier can use
        [Authorize(Policy = "Cashier")]
        [HttpPost]
        public async Task<ActionResult<Order>> AddOrder(AddOrderDTO orderDTO)
        {
            try
            {

                // Retrieve the token from the Authorization header
                var authorizationHeader = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
                {
                    return BadRequest("Authorization header is either empty or does not contain a Bearer token.");
                }

                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                var userID = _tokenService.GetUserIdFromTokenHeader(token);

                if (userID == Guid.Empty)
                {
                    // Handle the case where the userId claim is not a valid Guid
                    return BadRequest("Invalid user ID format.");
                }

                orderDTO.CustomerName = orderDTO.CustomerName?.Trim();

                // Check if all data is valid
                string? error = await _orderService.IsGenerallyValidated(orderDTO.ProductID, orderDTO.Quantity, orderDTO.CustomerName, orderDTO.CustomerPhone);

                if (error is not null)
                {
                    return BadRequest(error);
                }

                if (await _orderService.AddOrder(orderDTO, userID))
                {
                    return Ok(orderDTO);
                }

                return BadRequest("Unable to add order");
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }

        }


        // PUT: api/Orders
        // Update an order to database
        // Only Order Manager can use
        [Authorize(Policy = "Order")]
        [HttpPut]
        public async Task<ActionResult<Order>> UpdateOrder(UpdateOrderDTO orderDTO)
        {
            try
            {

                if (!await _orderService.IsValidOrder(orderDTO.ID))
                {
                    return BadRequest("Invalid Order ID");
                }

                orderDTO.Status = orderDTO.Status?.Trim(); //Modified status string,
                                                           //'   Hello, World!   ' -> 'Hello, World!'

                if (!_orderService.IsValidStatusFormat(orderDTO.Status))
                {
                    return BadRequest($"Status {orderDTO.Status} is not valid format");
                }

                if (await _orderService.UpdateOrder(orderDTO))
                {
                    return Ok(orderDTO);
                }

                return BadRequest("Unable to update order");
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex.Message);
            }
        }
    }
}