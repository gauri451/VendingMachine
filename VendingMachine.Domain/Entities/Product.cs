using VendingMachine.Domain.Enums;

namespace VendingMachine.Domain.Entities
{
    public class Product
    {
        public ProductType Type { get; }
        public decimal Price { get; }

        public Product(ProductType type)
        {
            Type = type;
            Price = type switch
            {
                ProductType.Cola => 1.00m,
                ProductType.Chips => 0.50m,
                ProductType.Candy => 0.65m,
                _ => throw new ArgumentException("Invalid product")
            };
        }
    }
}
