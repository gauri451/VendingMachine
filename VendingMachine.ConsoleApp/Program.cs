using System;
using System.Globalization;
using VendingMachine.Domain.Entities;
using VendingMachine.Domain.Enums;
using VendingMachine.Application;
using VendingMachine.Domain.Interfaces;

class Program
{
    static void Main()
    {
        ICoinService coinService = new CoinService();
        IProductService productService = new ProductService(coinService);

        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== VENDING MACHINE ===");
            Console.WriteLine("Products:");
            Console.WriteLine("1. Cola  - $1.00");
            Console.WriteLine("2. Chips - $0.50");
            Console.WriteLine("3. Candy - $0.65\n");

            Console.Write("Select a product (cola, chips, candy): ");
            var productInput = Console.ReadLine()?.ToLower();

            if (!Enum.TryParse(typeof(ProductType), CultureInfo.CurrentCulture.TextInfo.ToTitleCase(productInput), out var selectedProduct))
            {
                Console.WriteLine("Invalid product selected. Press ENTER to retry.");
                Console.ReadLine();
                continue;
            }

            var product = new Product((ProductType)selectedProduct);
            Console.WriteLine($"\nSelected product: {product.Type} - Price: ${product.Price:F2}");

            // Reset inserted coins for new transaction
            coinService.Reset();

            while (coinService.CurrentAmount < product.Price)
            {
                decimal amountNeeded = product.Price - coinService.CurrentAmount;
                Console.WriteLine($"\n[Display] INSERT COIN - Amount needed: ${amountNeeded:F2}");
                Console.Write("Insert coin (nickel, dime, quarter, penny) or type 'cancel': ");
                var input = Console.ReadLine()?.ToLower();

                if (input == "cancel")
                {
                    Console.WriteLine("Transaction cancelled. Returning coins...");
                    break;
                }

                if (Enum.TryParse(typeof(CoinType), CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input), out var coinType))
                {
                    var coin = new Coin((CoinType)coinType);
                    bool accepted = coinService.InsertCoin(coin);

                    if (!accepted)
                    {
                        Console.WriteLine($"Coin '{coin.Type}' is not accepted and moved to coin return.");
                    }
                    else
                    {
                        Console.WriteLine($"Inserted: {coin.Type} (${coin.Value:F2}) | Total inserted: ${coinService.CurrentAmount:F2}");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid coin.");
                }
            }

            if (coinService.CurrentAmount >= product.Price)
            {
                string dispenseMessage = productService.SelectProduct(product.Type);
                Console.WriteLine($"\n{dispenseMessage}");

                Console.WriteLine("\nReturning change:");
                foreach (var coin in coinService.GetCoinReturn())
                {
                    Console.WriteLine($"- {coin.Type} (${coin.Value:F2})");
                }
            }
            else
            {
                // If cancelled, return inserted coins
                Console.WriteLine("\nReturning inserted coins:");
                foreach (var coin in coinService.GetCoinReturn())
                {
                    Console.WriteLine($"- {coin.Type} (${coin.Value:F2})");
                }

                coinService.Reset();  // Reset AFTER displaying returned coins
            }

            Console.WriteLine("\nPress ENTER to continue or type 'exit' to quit.");
            if (Console.ReadLine()?.ToLower() == "exit")
                break;
        }
    }
}
