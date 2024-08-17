

using Microsoft.EntityFrameworkCore;
using SewingFactory.Models;
using SewingFactory.Models.DTO;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Interface;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace SewingFactory.Services.Service
{
    public class OrderService : IOrderService
    {
        private readonly DatabaseContext _dbContext;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly IRoleService _roleService;

        public OrderService(DatabaseContext dbContext, IUserService userService, IProductService productService, IRoleService roleService)
        {
            _dbContext = dbContext;
            _userService = userService;
            _productService = productService;
            _roleService = roleService;
        }

        public async Task<bool> AddOrder(AddOrderDTO dto)
        {
            var order = new Order()
            {
                ProductID = dto.ProductID,
                Quantity = dto.Quantity,
                OrderDate = DateTime.Now,
                FinishedDate = null,
                TotalAmount = dto.TotalAmount,
                UserID = dto.UserID,
                Status = "Not Started",
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<GetOrderDTO>> GetAllOrderDTOList()
        {
            IEnumerable<Order> orderList = await GetOrderList();
            var orders = new List<GetOrderDTO>();

            foreach (var order in orderList)
            {
                var orderDTO = new GetOrderDTO
                {
                    OrderDate = DateTime.Now,
                    FinishedDate = order.FinishedDate,
                    CustomerName = order.CustomerName,
                    CustomerPhone = order.CustomerPhone,
                    Quantity = order.Quantity,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    CreatorName = await _userService.GetUserName(order.UserID),
                    ProductName = await _productService.GetProductName(order.ProductID)
                };
                orders.Add(orderDTO);
            }

            return orders;
        }

        public async Task<Order> GetOrder(Guid orderID)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.ID == orderID);
            return order;
        }

        public async Task<GetOrderDTO> GetOrderDTO(Guid orderID)
        {
            var order = await GetOrder(orderID);
            GetOrderDTO orderDTO = new GetOrderDTO()
            {
                OrderDate = DateTime.Now,
                FinishedDate = order.FinishedDate,
                CustomerName = order.CustomerName,
                CustomerPhone = order.CustomerPhone,
                Quantity = order.Quantity,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                CreatorName = await _userService.GetUserName(order.UserID),
                ProductName = await _productService.GetProductName(order.ProductID)
            };

            return orderDTO;
        }

        public async Task<IEnumerable<Order>> GetOrderList()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task<string> IsGenerallyValidated(Guid userID)
        {
            if (await _userService.IsValidUser(userID) is false)
            {
                return "Invalid user";
            }

            var user = await _userService.GetUser(userID);
            var userRole = await _roleService.GetRoleName(user.RoleID);
            if (userRole != "Order Manager")
            {
                return "You don't have permission to use this feature";
            }
            return null;
        }

        public async Task<bool> IsValidOrder(Guid orderID)
        {
            var order = await GetOrder(orderID);
            if (order == null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateOrder(UpdateOrderDTO dto)
        {
            var order = await GetOrder(dto.ID);
            if (order is null) { return false; }

            // Update the order properties with values from the DTO
            order.UserID = dto.UserID;
            order.Status = dto.Status;

            if (dto.Status == "Done")
            {
                order.FinishedDate = DateTime.Now;
            }
            else
            {
                order.FinishedDate = null;
            }


            // Save changes asynchronously
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
