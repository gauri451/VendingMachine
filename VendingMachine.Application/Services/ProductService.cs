using VendingMachine.Domain.Entities;
using VendingMachine.Domain.Enums;
using VendingMachine.Domain.Interfaces;

namespace VendingMachine.Application
{
    public class ProductService : IProductService
    {
        private readonly ICoinService _coinService;

        public ProductService(ICoinService coinService)
        {
            _coinService = coinService;
        }

        public string SelectProduct(ProductType productType)
        {
            var product = new Product(productType);

            if (_coinService.CurrentAmount >= product.Price)
            {
                // Dispense product
                decimal change = _coinService.CurrentAmount - product.Price;

                // Return leftover change coins
                if (change > 0)
                {
                    _coinService.ReturnChange(change);
                }

                return $"THANK YOU! Dispensed {product.Type}";
            }
            else
            {
                decimal amountNeeded = product.Price - _coinService.CurrentAmount;
                return $"PRICE: ${product.Price:F2} - Please insert ${amountNeeded:F2} more";
            }
        }
    }
}
