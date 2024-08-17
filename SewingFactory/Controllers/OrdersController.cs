using Microsoft.AspNetCore.Mvc;
using SewingFactory.Models;
using SewingFactory.Models.DTO;
using SewingFactory.Services.Interface;

namespace SewingFactory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetAllOrders()
        {
            IEnumerable<GetOrderDTO> orderList = await _orderService.GetAllOrderDTOList();
            return Ok(orderList);
        }

        // GET: api/Orders/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetOrderDTO>> GetOrder(Guid id)
        {
            var order = await _orderService.GetOrderDTO(id);
            return Ok(order);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> AddOrder(AddOrderDTO orderDTO)
        {

            var userID = orderDTO.UserID;

            if (!await _orderService.IsValidUserForAddOrderFeature(userID)) // check if user is valid
            {
                return BadRequest("Invalid user or user doesn't have permission to access this feature!");
            }

            orderDTO.CustomerName = orderDTO.CustomerName?.Trim();

            // Check if all data is valid
            string? error = await _orderService.IsGenerallyValidated(orderDTO.ProductID, orderDTO.Quantity, orderDTO.CustomerName, orderDTO.CustomerPhone);

            if (error is not null)
            {
                return BadRequest(error);
            }

            if (await _orderService.AddOrder(orderDTO))
            {
                return Ok(orderDTO);
            }

            return BadRequest("Unable to add order");
        }


        // PUT: api/Orders
        [HttpPut]
        public async Task<ActionResult<Order>> UpdateOrder(UpdateOrderDTO orderDTO)
        {

            // Waiting for JWT

            //var userID = orderDTO.UserID;

            //if (!await _orderService.IsValidUser(userID)) // Check user authority
            //{
            //    return BadRequest("Invalid user or user doesn't have permission to access this feature!");
            //}

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
    }
}
