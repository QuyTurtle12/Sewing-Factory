using SewingFactory.Models;
using SewingFactory.Models.DTO;
using SewingFactory.Models.Models;

namespace SewingFactory.Services.Interface
{
    public interface IOrderService
    {
        //Task<IEnumerable<Order>> GetOrderList();

        //Task<Order> GetOrder(Guid orderID);

        Task<GetOrderDTO> GetOrderDTO(Guid orderID);

        Task<IEnumerable<GetOrderDTO>> GetAllOrderDTOList();

        Task<bool> IsValidOrder(Guid orderID);

        Task<bool> AddOrder(AddOrderDTO dto);

        Task<bool> UpdateOrder(UpdateOrderDTO dto);

        Task<bool> IsValidUserForAddOrderFeature(Guid userId);

        Task<bool> IsValidUserForUpdateOrderFeature(Guid userId);

        bool IsValidStatusFormat(string? status);

        Task<string?> IsGenerallyValidated(Guid productID, int? quantity, string? CustomerName, string? CustomerPhone);
    }
}