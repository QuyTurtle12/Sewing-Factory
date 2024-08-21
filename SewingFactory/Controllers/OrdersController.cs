using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SewingFactory.Models;
using SewingFactory.Models.DTOs;
using SewingFactory.Services.Interface;
using SewingFactory.Services.Service;
using Swashbuckle.AspNetCore.Annotations;

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

        // GET: api/Orders/page
        // Get all orders
        // Only Cashier or Order Manager can use
        [Authorize(Policy = "Cashier-Order")]
        [HttpGet]
        [Route("page")]
        [SwaggerOperation(
            Summary = "Authorization: Cashier & Order Manager",
            Description = "View order list in a page"
        )]
        public async Task<ActionResult<IEnumerable<OrderViewDto>>> GetOrdersInPage(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                IEnumerable<OrderViewDto> orderList = await _orderService.GetAllPagedOrderDTOList(pageNumber, pageSize);
                return Ok(orderList);
            }
            catch (Exception ex)
            {

                return StatusCode(500, "Invalid input" + ex.Message);
            }
        }

        // GET: api/Orders
        // Get all orders
        // Only Cashier or Order Manager can use
        [Authorize(Policy = "Cashier-Order")]
        [HttpGet]
        [SwaggerOperation(
            Summary = "Authorization: Cashier & Order Manager",
            Description = "View order list"
        )]
        public async Task<ActionResult<IEnumerable<OrderViewDto>>> GetAllOrders()
        {
            try
            {
                IEnumerable<OrderViewDto> orderList = await _orderService.GetAllOrderDTOList();
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
        [SwaggerOperation(
            Summary = "Authorization: Cashier & Order Manager",
            Description = "View an order by order id"
        )]
        public async Task<ActionResult<OrderViewDto>> GetOrder(Guid id)
        {
            try
            {
                if (!await _orderService.IsValidOrder(id))
                {
                    return BadRequest("Invalid Order ID");
                }
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
        [SwaggerOperation(
            Summary = "Authorization: Cashier & Order Manager",
            Description = "Search order list by a filter. Filter list: status, cashier id, customer phone, order date, finish date, total amount. Example format: " +
            "status: Not Started/In Progress/Done, " +
            "cashier id: 00000000-0000-0000-0000-000000000000, " +
            "customer phone: ###-####-### or ###-####-####, " +
            "order date: dd/MM/yyyy, " +
            "finish date: dd/MM/yyyy, " +
            "total amount: 1000000"
        )]
        public async Task<ActionResult<IEnumerable<OrderViewDto>>> SearchOrderList(int pageNumber = 1, int pageSize = 5, string? firstInputValue = null, string? secondInputValue = null, string filter = "status")
        {
            try
            {
                return Ok(await _orderService.searchOrderDTOList(pageNumber, pageSize, firstInputValue, secondInputValue, filter));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error: " + ex.Message);
            }
        }

        // POST: api/Orders
        // Add an order to database
        // Only Cashier can use
        [Authorize(Policy = "Cashier")]
        [HttpPost]
        [SwaggerOperation(
            Summary = "Authorization: Cashier",
            Description = "Add order"
        )]
        public async Task<ActionResult<Order>> AddOrder(OrderCreateDto orderDTO)
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
        [SwaggerOperation(
            Summary = "Authorization: Order Manager",
            Description = "Update Order Status"
        )]
        public async Task<ActionResult<Order>> UpdateOrder(OrderUpdateDto orderDTO)
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