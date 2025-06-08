using System.Collections.Generic;
using VendingMachine.Domain.Entities;

namespace VendingMachine.Domain.Interfaces
{
    public interface ICoinService
    {
        decimal CurrentAmount { get; }

        /// <summary>
        /// Inserts a coin. Returns true if coin accepted, false if rejected.
        /// </summary>
        /// <param name="coin"></param>
        /// <returns></returns>
        bool InsertCoin(Coin coin);

        /// <summary>
        /// Returns the list of coins currently in the coin return slot.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Coin> GetCoinReturn();

        /// <summary>
        /// Adds change coins to coin return.
        /// </summary>
        /// <param name="changeAmount"></param>
        void ReturnChange(decimal changeAmount);

        /// <summary>
        /// Resets the service for a new transaction.
        /// </summary>
        void Reset();
    }
}
