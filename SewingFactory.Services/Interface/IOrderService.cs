using SewingFactory.Models;
using SewingFactory.Models.DTOs;

namespace SewingFactory.Services.Interface
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrderList();

        Task<Order> GetOrder(Guid orderID);

        Task<OrderViewDto> GetOrderDTO(Guid orderID);

        Task<IEnumerable<OrderViewDto>> GetAllPagedOrderDTOList(int pageNumber, int pageSize);

        Task<IEnumerable<OrderViewDto>> GetAllOrderDTOList();

        Task<bool> IsValidOrder(Guid orderID);

        Task<bool> AddOrder(OrderCreateDto dto, Guid userID);

        Task<bool> UpdateOrder(OrderUpdateDto dto);

        bool IsValidStatusFormat(string? status);

        Task<string?> IsGenerallyValidated(Guid productID, int? quantity, string? CustomerName, string? CustomerPhone);

        Task<IEnumerable<OrderViewDto>> searchOrderDTOList(int pageNumber, int pageSize, string? firstInputValue, string? secondInputValue, string filter);
    }
}