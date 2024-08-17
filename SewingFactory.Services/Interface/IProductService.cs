﻿

namespace SewingFactory.Services.Interface
{
    public interface IProductService
    {
        Task<bool> IsValidProduct(Guid productID);

        Task<string> GetProductName(Guid productID);
    }
}
