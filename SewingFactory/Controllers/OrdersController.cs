using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SewingFactory.Models;
using SewingFactory.Models.DTO;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Interface;

namespace SewingFactory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        private readonly IUserService _userService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public OrdersController(DatabaseContext dbcontext, IUserService userService, IOrderService orderService, IProductService productService)
        {
            _dbContext = dbcontext;
            _userService = userService;
            _orderService = orderService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetOrderDTO>>> GetAllOrders()
        {
            IEnumerable<GetOrderDTO> orderList = await _orderService.GetAllOrderDTOList();
            return Ok(orderList);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<GetOrderDTO>> GetOrder(Guid id)
        {
            var order = await _orderService.GetOrderDTO(id);
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> AddOrder(AddOrderDTO orderDto)
        {

            var userID = orderDto.UserID;
            IEnumerable<User> userList = await _dbContext.Users.ToListAsync();

            string error = await _orderService.IsGenerallyValidated(userID); //Need validate product

            if (error is not null)
            {
                return BadRequest(error);
            }

            if (await _orderService.AddOrder(orderDto))
            {
                return Ok(orderDto);
            }

            return BadRequest("Unable to add order");
        }


        [HttpPut]
        public async Task<ActionResult<Order>> UpdateOrder(UpdateOrderDTO orderDto)
        {

            var userID = orderDto.UserID;
            IEnumerable<User> userList = await _dbContext.Users.ToListAsync();

            string error = await _orderService.IsGenerallyValidated(userID);

            if (error is not null)
            {
                return BadRequest(error);
            }

            if (await _orderService.UpdateOrder(orderDto))
            {
                return Ok(orderDto);
            }

            return BadRequest("Unable to update order");
        }
    }
}
