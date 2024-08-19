using Microsoft.EntityFrameworkCore;
using SewingFactory.Core;
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
        private readonly UserService _userService;
        private readonly IProductService _productService;

        public OrderService(DatabaseContext dbContext, UserService userService, IProductService productService)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        // Add a new order to database
        public async Task<bool> AddOrder(AddOrderDTO dto, Guid userID)
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
                UserID = userID, // User who created the order
                Status = "Not Started",
                CustomerName = dto.CustomerName,
                CustomerPhone = dto.CustomerPhone
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync(); // Save changes asynchronously to database

            return true;
        }

        // Show all orders info at human view point
        public async Task<IEnumerable<GetOrderDTO>> GetAllOrderDTOList(int pageNumber, int pageSize)
        {
            IEnumerable<Order> orderList = await GetOrderList();
            var orderDtos = new List<GetOrderDTO>();

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
                orderDtos.Add(orderDTO);
            }

            var orderPaginationList = new PaginatedList<GetOrderDTO>(orderDtos, pageNumber, pageSize);

            return orderPaginationList.GetPaginatedItems();
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

            // Transfer order data to showable data at human view-point
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