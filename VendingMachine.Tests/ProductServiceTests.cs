using VendingMachine.Application;
using VendingMachine.Domain.Entities;
using VendingMachine.Domain.Enums;
using VendingMachine.Domain.Interfaces;
using Xunit;

public class ProductServiceTests
{
    [Fact]
    public void SelectProduct_InsufficientFunds_ShowsPriceAndNeededAmount()
    {
        ICoinService coinService = new CoinService();
        IProductService productService = new ProductService(coinService);

        var productType = ProductType.Candy; // $0.65

        var messageBefore = productService.SelectProduct(productType);

        coinService.InsertCoin(new Coin(CoinType.Nickel)); // $0.05
        coinService.InsertCoin(new Coin(CoinType.Dime));   // $0.10

        var messageAfter = productService.SelectProduct(productType);

        Assert.Equal("PRICE: $0.65 - Please insert $0.65 more", messageBefore);
        Assert.Equal("PRICE: $0.65 - Please insert $0.50 more", messageAfter);
    }

    [Fact]
    public void SelectProduct_ExactFunds_DispensesProductAndResets()
    {
        ICoinService coinService = new CoinService();
        IProductService productService = new ProductService(coinService);

        var productType = ProductType.Chips; // $0.50

        coinService.InsertCoin(new Coin(CoinType.Quarter));
        coinService.InsertCoin(new Coin(CoinType.Quarter));

        var message = productService.SelectProduct(productType);

        Assert.Equal("THANK YOU! Dispensed Chips", message);
        Assert.Equal(0m, coinService.CurrentAmount);
    }

    [Fact]
    public void SelectProduct_MoreThanFunds_ReturnsChange()
    {
        ICoinService coinService = new CoinService();
        IProductService productService = new ProductService(coinService);

        var productType = ProductType.Chips; // $0.50

        // Insert $1.00
        for (int i = 0; i < 4; i++)
            coinService.InsertCoin(new Coin(CoinType.Quarter));

        var message = productService.SelectProduct(productType);

        Assert.Equal("THANK YOU! Dispensed Chips", message);

        var coinReturn = coinService.GetCoinReturn();

        Assert.NotEmpty(coinReturn);

        // Check that coin return has 2 quarters (0.50)
        int quarterCount = 0;
        foreach (var coin in coinReturn)
            if (coin.Type == CoinType.Quarter)
                quarterCount++;

        Assert.Equal(2, quarterCount);
    }
}
