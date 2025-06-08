using System.Linq;
using VendingMachine.Application;
using VendingMachine.Domain.Entities;
using VendingMachine.Domain.Enums;
using VendingMachine.Domain.Interfaces;
using Xunit;

public class CoinServiceTests
{
    [Fact]
    public void InsertValidCoins_IncreasesCurrentAmount()
    {
        ICoinService coinService = new CoinService();

        Assert.True(coinService.InsertCoin(new Coin(CoinType.Nickel)));
        Assert.True(coinService.InsertCoin(new Coin(CoinType.Dime)));
        Assert.True(coinService.InsertCoin(new Coin(CoinType.Quarter)));

        Assert.Equal(0.40m, coinService.CurrentAmount); // 0.05 + 0.10 + 0.25 = 0.40
    }

    [Fact]
    public void InsertInvalidCoin_IsRejected()
    {
        ICoinService coinService = new CoinService();

        bool result = coinService.InsertCoin(new Coin(CoinType.Penny));

        Assert.False(result);
        Assert.Equal(0m, coinService.CurrentAmount);

        var coinReturn = coinService.GetCoinReturn();
        Assert.Single(coinReturn);
        Assert.Equal(CoinType.Penny, coinReturn.First().Type);
    }

    [Fact]
    public void ReturnChange_AddsCoinsToCoinReturn()
    {
        ICoinService coinService = new CoinService();

        coinService.ReturnChange(0.40m); // should return 1 quarter, 1 dime, 1 nickel

        var coinsReturned = coinService.GetCoinReturn().ToList();

        Assert.Contains(coinsReturned, c => c.Type == CoinType.Quarter);
        Assert.Contains(coinsReturned, c => c.Type == CoinType.Dime);
        Assert.Contains(coinsReturned, c => c.Type == CoinType.Nickel);

        // Total returned should sum up to 0.40
        decimal totalReturned = coinsReturned.Sum(c => c.Value);
        Assert.Equal(0.40m, totalReturned);
    }

    [Fact]
    public void Reset_ClearsCurrentAmountAndCoinReturn()
    {
        ICoinService coinService = new CoinService();

        coinService.InsertCoin(new Coin(CoinType.Quarter));
        coinService.ReturnChange(0.10m);

        coinService.Reset();

        Assert.Equal(0m, coinService.CurrentAmount);
        Assert.Empty(coinService.GetCoinReturn());
    }
}
