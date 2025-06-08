# Vending Machine Console Application

## Overview

This is a simple console-based vending machine simulation built with .NET Core.  
It allows users to:

- Select a single product per transaction (Cola, Chips, Candy)  
- Insert coins repeatedly until enough money is inserted  
- Dispense the selected product if sufficient funds are provided  
- Return change using the largest coins possible (quarters, dimes, nickels, pennies)  
- Reject invalid coins (pennies) on insertion and place them in the coin return  

## Features

- **Coin Recognition:** Accepts nickels (0.05$), dimes (0.10$), quarters (0.25$), rejects pennies (0.01$).  
- **Product Pricing:**  
  - Cola: $1.00  
  - Chips: $0.50  
  - Candy: $0.65  
- **Change Return:** Returns leftover change after purchase with coins, prioritizing largest coin denominations.  
- **User Interaction:**  
  - User selects one product at a time.  
  - User inserts coins until total amount >= product price.  
  - Displays prompts showing inserted amount and required amount.  
  - Allows transaction cancellation, returning inserted coins.  

## Technologies Used

- .NET Core Console Application  
- Clean separation of concerns using interfaces and classes  
- Basic domain-driven design concepts applied  
- Enum-based coin and product definitions  

## Folder Structure

