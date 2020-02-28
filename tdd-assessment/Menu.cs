using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdd_assessment
{
    public class Menu
    {
        private Function function;

        public void RunMenu()
        {
            function = new Function(RestockItems());

            while (true)
            {
                Console.Clear();
                Console.WriteLine("(1) Display Vending Machine Items");
                Console.WriteLine("(2) Purchase");            
                Console.WriteLine("(3) Exit and Leave \n");
                Console.Write("Please make a selection   >>  ");
                string userChoose = Console.ReadLine();

                if (userChoose == "1")
                {
                    Console.Clear();
                    List<string> itemsToPrint = function.ListAllItems();
                    function.ShowProducts(itemsToPrint);
                    Console.ReadLine();
                }
                else if (userChoose == "2") {
                    Console.Clear();
                    new Buy(function);
                }          
                else if (userChoose == "3")
                {
                    break;
                }
            }
        }

        public Dictionary<string, Stock> RestockItems()
        {
            Dictionary<string, Stock> stocks = new Dictionary<string, Stock>();
            const int maxNumItemsPerSlot = 5;
            Stock stock;

            string[] items = { "1|Coke|25", "2|Pepsi|35", "3|Soda|45", "4|Chocolate Bar|20.25", "5|Chewing Gum|10.50", "6|Bottled Water|15" };

            foreach (var item in items)
            {
                string[] itemInformation = item.Split('|');
                string stockId = itemInformation[0];
                string productName = itemInformation[1];
                decimal price = decimal.Parse(itemInformation[2]);

                List<Item> chips = new List<Item>();
                for (int i = 0; i < maxNumItemsPerSlot; i++)
                {
                    chips.Add(new Item { ItemId = stockId, ProductName = productName, Price = price });
                }

                stock = new Stock(stockId, chips);
                stocks[stockId] = stock;
            }

            return stocks;
        }

    }
}
