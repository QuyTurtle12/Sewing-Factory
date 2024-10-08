﻿using Microsoft.EntityFrameworkCore;
using SewingFactory.Core;
using SewingFactory.Models;
using SewingFactory.Models.DTOs;
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
        public async Task<bool> AddOrder(OrderCreateDto dto, Guid userID)
        {
            var product = await _productService.GetProductAsync(dto.ProductID);
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
        public async Task<IEnumerable<OrderViewDto>> GetAllPagedOrderDTOList(int pageNumber, int pageSize)
        {
            try
            {
                IEnumerable<Order> orderList = await GetOrderList();
                var orderDtos = new List<OrderViewDto>();

                foreach (var order in orderList)
                {
                    var orderDTO = new OrderViewDto
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

                var orderPaginationList = new PaginatedList<OrderViewDto>(orderDtos, pageNumber, pageSize);

                return orderPaginationList.GetPaginatedItems();
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Specific handling for pagination issues
                throw new Exception("Invalid pagination parameters: " + ex.Message);
            }
            catch (Exception e)
            {
                throw new Exception("Error: " + e.Message);
            }
            
        }

        public async Task<IEnumerable<OrderViewDto>> GetAllOrderDTOList()
        {
            IEnumerable<Order> orderList = await GetOrderList();
            var orderDtos = new List<OrderViewDto>();

            foreach (var order in orderList)
            {
                var orderDTO = new OrderViewDto
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

            return orderDtos;
        }


        public async Task<Order> GetOrder(Guid orderID)
        {
            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.ID == orderID);
            return order;
        }

        // Show order info at human view point
        public async Task<OrderViewDto> GetOrderDTO(Guid orderID)
        {
            var order = await GetOrder(orderID);

            // Transfer order data to showable data at human view-point
            OrderViewDto orderDTO = new OrderViewDto()
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
            var product = await _productService.GetProductAsync(productID);
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

        // Check if order is existed
        public async Task<bool> IsValidOrder(Guid orderID)
        {
            var order = await GetOrder(orderID);
            if (order == null)
            {
                return false;
            }
            return true;
        }

        // Check if status is correct format
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

        // Get a list of order by a specific filter
        public async Task<IEnumerable<OrderViewDto>> searchOrderDTOList(int pageNumber, int pageSize, string? firstInputValue, string? secondInputValue, string filter)
        {
            try
            {
                IEnumerable<Order> orders = await GetOrderList();

                // Modified variable
                filter = filter.Trim().ToLower() ?? string.Empty;
                firstInputValue = firstInputValue?.Trim() ?? null;
                secondInputValue = secondInputValue?.Trim() ?? null;

                // Create an empty list
                IEnumerable<OrderViewDto> result = new List<OrderViewDto>();

                // variable for using only one input
                string? inputValue = string.Empty;

                // Choosing the input that not null
                if (!string.IsNullOrWhiteSpace(firstInputValue) || !string.IsNullOrWhiteSpace(secondInputValue))
                {
                    // If the first value not null then choose the first value else the second value
                    inputValue = firstInputValue ?? secondInputValue;
                }


                switch (filter)
                {
                    case "status": // Search orders by status
                        result = await StatusFilterAsync(inputValue, orders);
                        break;
                    case "cashier id": // Search orders by cashier id
                        result = await CashierIDFilterAsync(inputValue, orders);
                        break;
                    case "customer phone": // Search orders by customer phone
                        result = await CustomerPhoneFilterAsync(inputValue, orders);
                        break;
                    case "order date": // Search orders by order date
                        result = await DateFilterAsync(firstInputValue, secondInputValue, filter, orders);
                        break;
                    case "finish date": // Search orders by finish date
                        result = await DateFilterAsync(firstInputValue, secondInputValue, filter, orders);
                        break;
                    case "total amount": // Search orders by total amount
                        result = await GetTotalAmountInRange(filter, orders, Double.Parse(firstInputValue), Double.Parse(secondInputValue));
                        break;
                    default:
                        throw new ArgumentException($"Invalid {nameof(filter)}: {filter}. Allowed filters are 'status', 'cashier id', 'customer phone', 'order date', 'finish date', 'total amount'.");
                }
                // Create a new PaginatedList object, which handles the pagination logic for the results.
                var orderPaginationList = new PaginatedList<OrderViewDto>(result, pageNumber, pageSize);

                // Retrieve the paginated items from the PaginatedList.
                return orderPaginationList.GetPaginatedItems();
            }
            catch (FormatException ex)
            {
                // Specific handling for format issues
                throw new Exception("Invalid format: " + ex.Message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Specific handling for pagination issues
                throw new Exception("Invalid pagination parameters: " + ex.Message);
            }
            catch (Exception ex)
            {
                // General exception handling
                throw new Exception("An error occurred: " + ex.Message);
            }


        }

        // Get order list by status
        private async Task<IEnumerable<OrderViewDto>> StatusFilterAsync(string? inputValue, IEnumerable<Order> orders)
        {
            List<OrderViewDto> result = new List<OrderViewDto>();
            switch (inputValue)
            {
                case "Not Started":
                    foreach (Order order in orders)
                    {
                        // Check if order status is not null and equal to "Not Started"
                        if (!(order.Status?.Equals(inputValue) ?? false))
                        {
                            continue; // Skip the whole code below and move to next iteration
                        }
                        // Transfer entity data to dto value that human understand
                        OrderViewDto orderDTO = new OrderViewDto()
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
                        result.Add(orderDTO);
                    }
                    return result;
                case "In Progress":
                    foreach (Order order in orders)
                    {
                        // Check if order status is not null and equal to "In Progress"
                        if (!(order.Status?.Equals(inputValue) ?? false))
                        {
                            continue; // Skip the whole code below and move to next iteration
                        }

                        // Transfer entity data to dto value that human understand
                        OrderViewDto orderDTO = new OrderViewDto()
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
                        result.Add(orderDTO);
                    }

                    return result;
                case "Done":
                    foreach (Order order in orders)
                    {
                        // Check if order status is not null and equal to "Done"
                        if (!(order.Status?.Equals(inputValue) ?? false))
                        {
                            continue; // Skip the whole code below and move to next iteration
                        }

                        // Transfer entity data to dto value that human understand
                        OrderViewDto orderDTO = new OrderViewDto()
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
                        result.Add(orderDTO);
                    }

                    return result;
                default:
                    throw new ArgumentException("Invalid Status: The provided status is not valid.");
            }
        }

        // Get order list by cashier id
        private async Task<IEnumerable<OrderViewDto>> CashierIDFilterAsync(string? inputValue, IEnumerable<Order> orders)
        {
            // Validate the input value
            if (string.IsNullOrWhiteSpace(inputValue) || !Guid.TryParse(inputValue, out Guid cashierID))
            {
                throw new ArgumentException("Invalid Cashier ID: The provided ID is not a valid GUID.");
            }

            List<OrderViewDto> result = new List<OrderViewDto>();

            foreach (Order order in orders)
            {
                // Check if input value can be parsed as a GUID and equal to order's user id
                if (!Guid.TryParse(inputValue, out Guid inputGuid) || !order.UserID.Equals(inputGuid))
                {
                    continue; // Skip the whole code below and move to next iteration
                }

                // Transfer entity data to dto value that human understand
                OrderViewDto orderDTO = new OrderViewDto()
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
                result.Add(orderDTO);
            }

            return result;
        }

        // Get order list by customer phone
        private async Task<IEnumerable<OrderViewDto>> CustomerPhoneFilterAsync(string? inputValue, IEnumerable<Order> orders)
        {
            // Regular expression for phone number format: ###-####-### or ###-####-####
            string phonePattern = @"^\d{3}-\d{4}-\d{3}$|^\d{3}-\d{4}-\d{4}$";

            // Validate the input phone number format
            if (!string.IsNullOrEmpty(inputValue) && !System.Text.RegularExpressions.Regex.IsMatch(inputValue, phonePattern))
            {
                throw new FormatException("Invalid phone number format. Please use ###-####-### or ###-####-####.");
            }

            List<OrderViewDto> result = new List<OrderViewDto>();

            foreach (Order order in orders)
            {
                // Check if customer phone is equal to input value (null also count as false)
                // Phone Number Format: ###-####-### or ###-####-####
                if (!(order.CustomerPhone?.Equals(inputValue)) ?? false)
                {
                    continue; // Skip the whole code below and move to next iteration
                }

                // Transfer entity data to dto value that human understand
                OrderViewDto orderDTO = new OrderViewDto()
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
                result.Add(orderDTO);
            }

            return result;
        }

        // Get order list by date
        private async Task<IEnumerable<OrderViewDto>> DateFilterAsync(string? startDate, string? endDate, string filter, IEnumerable<Order> orders)
        {
            // Define the date format
            string dateFormat = "dd/MM/yyyy";

            // Parse the startDate and endDate
            DateTime? startDateParsed = null;
            DateTime? endDateParsed = null;

            // Parse the startDate
            if (!string.IsNullOrWhiteSpace(startDate))
            {
                if (DateTime.TryParseExact(startDate, dateFormat, null, System.Globalization.DateTimeStyles.None, out DateTime startDateValue))
                {
                    startDateParsed = startDateValue;
                }
                else
                {
                    // Handle invalid date format
                    throw new ArgumentException("Invalid start date format. Please use " + dateFormat);
                }
            }

            // Parse the endDate
            if (!string.IsNullOrWhiteSpace(endDate))
            {
                if (DateTime.TryParseExact(endDate, dateFormat, null, System.Globalization.DateTimeStyles.None, out DateTime endDateValue))
                {
                    endDateParsed = endDateValue;
                }
                else
                {
                    // Handle invalid date format
                    throw new ArgumentException("Invalid end date format. Please use " + dateFormat);
                }
            }

            // Create an empty list
            IEnumerable<OrderViewDto> result = new List<OrderViewDto>();

            result = await GetDateInRange(filter, orders, startDateParsed, endDateParsed);

            return result;
        }

        // Encapsulate get order list by date process
        // Interact with database in here
        private async Task<List<OrderViewDto>> GetDateInRange(string filter, IEnumerable<Order> orders, DateTime? startDateParsed, DateTime? endDateParsed)
        {
            List<OrderViewDto> result = new List<OrderViewDto>();

            foreach (Order order in orders)
            {
                // Determine which date to use for filtering
                DateTime? dateToCheck = filter.Equals("order date")
                    ? order.OrderDate
                    : order.FinishedDate;

                // If the date is not in range, skip the order
                if (dateToCheck is null || !IsDateInRange(dateToCheck, startDateParsed, endDateParsed))
                {
                    continue;
                }

                // Transfer entity data to DTO
                OrderViewDto orderDTO = new OrderViewDto
                {
                    OrderDate = order.OrderDate,
                    FinishedDate = order.FinishedDate,
                    CustomerName = order.CustomerName,
                    CustomerPhone = order.CustomerPhone,
                    Quantity = order.Quantity,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    CreatorName = await _userService.GetUserName(order.UserID),
                    ProductName = await _productService.GetProductName(order.ProductID)
                };

                result.Add(orderDTO);
            }

            return result;
        }

        // Check if a date is within the specified range
        private bool IsDateInRange(DateTime? dateToCheck, DateTime? startDate, DateTime? endDate)
        {
            // Check if date is before start date or after end date
            return !((startDate.HasValue && dateToCheck < startDate) ||
                     (endDate.HasValue && dateToCheck > endDate));
        }

        // Get order list by total amount
        private async Task<List<OrderViewDto>> GetTotalAmountInRange(string filter, IEnumerable<Order> orders, double? minTotalAmount, double? maxTotalAmount)
        {
            // Validate the total amount range
            if (minTotalAmount == null || maxTotalAmount == null)
            {
                throw new ArgumentException("Invalid total amount: Both minimum and maximum total amounts must be provided.");
            }

            if (minTotalAmount < 0 || maxTotalAmount < 0)
            {
                throw new ArgumentException("Invalid total amount: Total amounts cannot be negative.");
            }

            if (minTotalAmount > maxTotalAmount)
            {
                throw new ArgumentException("Invalid total amount: Minimum total amount cannot be greater than the maximum total amount.");
            }

            // Create an empty list
            List<OrderViewDto> result = new List<OrderViewDto>();

            foreach (Order order in orders)
            {
                // Check if the salary is within the range
                if (!IsAmountInRange(order.TotalAmount, minTotalAmount, maxTotalAmount))
                {
                    continue; // Skip this order and move to the next one
                }

                // Transfer entity data to DTO value
                OrderViewDto orderDTO = new OrderViewDto()
                {
                    OrderDate = order.OrderDate,
                    FinishedDate = order.FinishedDate,
                    CustomerName = order.CustomerName,
                    CustomerPhone = order.CustomerPhone,
                    Quantity = order.Quantity,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    CreatorName = await _userService.GetUserName(order.UserID),
                    ProductName = await _productService.GetProductName(order.ProductID)
                };

                result.Add(orderDTO);
            }

            return result;
        }

        // Check if total amount is in given range
        private bool IsAmountInRange(double? amountToCheck, double? minTotalAmount, double? maxTotalAmount)
        {
            // Check if amount is lower than min value or higher than max value
            if ((minTotalAmount.HasValue && amountToCheck < minTotalAmount.Value) ||
                (maxTotalAmount.HasValue && amountToCheck > maxTotalAmount.Value))
            {
                return false;
            }

            return true;
        }

        // Update status to existed order
        public async Task<bool> UpdateOrder(OrderUpdateDto dto)
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