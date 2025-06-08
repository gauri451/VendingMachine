using System.Collections.Generic;
using System.Linq;
using VendingMachine.Domain.Entities;
using VendingMachine.Domain.Enums;
using VendingMachine.Domain.Interfaces;

namespace VendingMachine.Application
{
    public class CoinService : ICoinService
    {
        private readonly List<Coin> _insertedCoins = new();
        private readonly List<Coin> _coinReturn = new();

        public decimal CurrentAmount => _insertedCoins.Sum(c => c.Value);

        // Accept only nickels, dimes, quarters
        public bool InsertCoin(Coin coin)
        {
            if (coin.Type == CoinType.Penny)
            {
                // Reject penny immediately to coin return
                _coinReturn.Add(coin);
                return false;
            }
            else
            {
                _insertedCoins.Add(coin);
                return true;
            }
        }

        // Returns all coins in coin return (rejected + leftover change)
        public IEnumerable<Coin> GetCoinReturn()
        {
            return _coinReturn.ToList();
        }

        // Call after product dispense to return leftover change coins
        public void ReturnChange(decimal changeAmount)
        {
            // Return coins from largest to smallest
            int remainingCents = (int)(changeAmount * 100);

            int[] coinValues = { 25, 10, 5 };
            foreach (var coinValue in coinValues)
            {
                while (remainingCents >= coinValue)
                {
                    remainingCents -= coinValue;
                    CoinType coinType = coinValue switch
                    {
                        25 => CoinType.Quarter,
                        10 => CoinType.Dime,
                        5 => CoinType.Nickel,
                        _ => CoinType.Penny // Should never happen here
                    };

                    _coinReturn.Add(new Coin(coinType));
                }
            }

            // If leftover pennies (rare), return pennies as well
            for (int i = 0; i < remainingCents; i++)
            {
                _coinReturn.Add(new Coin(CoinType.Penny));
            }
        }

        // Reset for new transaction
        public void Reset()
        {
            _insertedCoins.Clear();
            _coinReturn.Clear();
        }
    }
}
