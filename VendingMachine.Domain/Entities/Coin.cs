using VendingMachine.Domain.Enums;

namespace VendingMachine.Domain.Entities
{
    public class Coin
    {
        public CoinType Type { get; }
        public decimal Value { get; }

        public Coin(CoinType type)
        {
            Type = type;
            Value = type switch
            {
                CoinType.Nickel => 0.05m,
                CoinType.Dime => 0.10m,
                CoinType.Quarter => 0.25m,
                _ => 0.00m
            };
        }

        public bool IsValid => Value > 0;
    }
}
