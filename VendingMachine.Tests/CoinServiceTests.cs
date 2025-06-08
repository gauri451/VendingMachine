using VendingMachine.Domain.Entities;
using VendingMachine.Domain.Enums;

namespace VendingMachine.Tests
{
    public class CoinServiceTests
    {
        [Fact]
        public void InsertValidCoin_ShouldUpdateAmount()
        {
            var service = new CoinService();
            service.InsertCoin(new Coin(CoinType.Quarter));

            Assert.Equal(0.25m, service.CurrentAmount);
        }

        [Fact]
        public void InsertInvalidCoin_ShouldGoToCoinReturn()
        {
            var service = new CoinService();
            service.InsertCoin(new Coin(CoinType.Invalid));

            Assert.Equal(0m, service.CurrentAmount);
            Assert.Single(service.GetCoinReturn());
        }

        [Fact]
        public void Display_ShouldShowCorrectAmount()
        {
            var service = new CoinService();
            service.InsertCoin(new Coin(CoinType.Dime));

            Assert.Equal("$0.10", service.GetDisplayMessage());
        }
    }

}
