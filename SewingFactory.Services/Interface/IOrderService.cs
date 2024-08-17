

using SewingFactory.Models;
using SewingFactory.Models.DTO;

namespace SewingFactory.Services.Interface
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrderList();

        Task<Order> GetOrder(Guid orderID);

        Task<GetOrderDTO> GetOrderDTO(Guid orderID);

        Task<IEnumerable<GetOrderDTO>> GetAllOrderDTOList();

        Task<bool> IsValidOrder(Guid orderID);

        Task<bool> AddOrder(AddOrderDTO dto);
        
        Task<bool> UpdateOrder(UpdateOrderDTO dto);

        Task<string> IsGenerallyValidated(Guid userID);
    }
}
