using VendingMachine.Domain.Enums;

namespace VendingMachine.Domain.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Process product selection and dispense if enough funds.
        /// Returns status message.
        /// </summary>
        /// <param name="productType"></param>
        /// <returns></returns>
        string SelectProduct(ProductType productType);
    }
}
