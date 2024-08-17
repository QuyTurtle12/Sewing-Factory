

using Microsoft.EntityFrameworkCore;
using SewingFactory.Models;
using SewingFactory.Models.DTO;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Interface;
using System.Text.RegularExpressions;

namespace SewingFactory.Services.Service
{
    public class OrderService : IOrderService
    {
        private readonly DatabaseContext _dbContext;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
    
        //Predefined constants
        private readonly Guid ORDER_MANAGER_ROLE_ID = new Guid("B4811F59-6537-41A9-890D-D41F8A7475A8");

        private readonly Guid CASHIER_ROLE_ID = new Guid("9D621DC0-FF6F-47B7-87E2-758383F4B13F");

        public OrderService(DatabaseContext dbContext, IUserService userService, IProductService productService)
        {
            _dbContext = dbContext;
            _userService = userService;
            _productService = productService;
        }

        // Add a new order to database
        public async Task<bool> AddOrder(AddOrderDTO dto)
        {
            var product = await _productService.GetProduct(dto.ProductID);
            double? totalAmount = product.Price * dto.Quantity;

            var order = new Order()
            {
                ProductID = dto.ProductID,
                Quantity = dto.Quantity,
                OrderDate = DateTime.Now,
                FinishedDate = null,
                TotalAmount = totalAmount,
                UserID = dto.UserID,
                Status = "Not Started",
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync(); // Save changes asynchronously to database

            return true;
        }

        // Show all orders info at human view point
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

        // Show order info at human view point
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

        // Validate all basic data that most methods in order controller use
        public async Task<string?> IsGenerallyValidated(Guid productID, int? quantity, string? CustomerName, string? CustomerPhone)
        {
            // Validate Product if they are null or disable
            var product = await _productService.GetProduct(productID);
            if (!(product?.Status ?? false)) 
            {
                return "Invalid Product";
            }

            // Validate Quantity
            if (quantity < 1)
            {
                return "Invalid Quantity";
            }

            // Validate CustomerName
            if (string.IsNullOrWhiteSpace(CustomerName))
            {
                return "Customer Name is required";
            }

            // Only accept character in CustomerName 
            var nameRegex = new Regex(@"^[a-zA-Z]+$");
            if (!nameRegex.IsMatch(CustomerName))
            {
                return "Customer Name must contain only alphabetic characters";
            }

            // Validate CustomerPhone
            if (string.IsNullOrWhiteSpace(CustomerPhone))
            {
                return "Customer Phone is required";
            }

            // Validate phone number format (555-6783-89731)
            var phoneRegex = new Regex(@"^\d{3}-\d{4}-\d{3,4}$");
            if (!phoneRegex.IsMatch(CustomerPhone))
            {
                return "Invalid Phone Number format. Use ###-####-### or ###-####-####";
            }

            return null; // Null mean no error
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

        public bool IsValidStatusFormat(string? status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                return false;
            }

            switch (status) // Check status string format
            {
                case "Not Started":
                    return true;
                case "In Progress":
                    return true;
                case "Done":
                    return true;
                default:
                    return false;
            }
        }

        // Check if the user who uses the method is valid user with cashier role
        public async Task<bool> IsValidUserForAddOrderFeature(Guid userId)
        {
            var user = await _userService.GetUser(userId);
            if (user?.RoleID == CASHIER_ROLE_ID)
            {
                return true;
            }
            return false;
        }

        // Check if the user who uses the method is valid user with order manager role
        public async Task<bool> IsValidUserForUpdateOrderFeature(Guid userId)
        {
            var user = await _userService.GetUser(userId);
            if (user?.RoleID == ORDER_MANAGER_ROLE_ID)
            {
                return true;
            }
            return false;
        }

        // Update status to existed order
        public async Task<bool> UpdateOrder(UpdateOrderDTO dto)
        {
            var order = await GetOrder(dto.ID);
            if (order is null) { return false; }

            // Update the order properties with values from the DTO
            order.Status = dto.Status;

            if (dto.Status == "Done") // Set the Finish Date if the order is finished
            {
                order.FinishedDate = DateTime.Now;
            }
            else
            {
                order.FinishedDate = null;
            }


            // Save changes asynchronously to database
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
