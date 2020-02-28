using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdd_assessment
{
    public class Buy
    {
        private Function function;

        public Buy(Function function) {
            this.function = function;
            DisplayPurchaseOptions();
        }

        public void DisplayPurchaseOptions()
        {
            while (true)
            {
                Console.WriteLine("(1)  Feed Money");
                Console.WriteLine("(2)  Select Product");
                Console.WriteLine("(3)  Refund");
                Console.WriteLine("(4)  Finish Transaction\n");
                Console.WriteLine("Current Money Provided : " + function.GetBalance() + '\n');
                Console.Write("Please make a selection   >> ");
                string userOption = Console.ReadLine();
                if (userOption == "1")
                {
                    Console.Clear();             
                    FeedMoney();
                }
                else if (userOption == "2")
                {
                    Console.Clear();
                    List<string> itemsToPrint = function.ListAllItems();
                    function.ShowProducts(itemsToPrint);
                    SelectProduct();
                }
                else if (userOption == "3")
                {
                    Console.Clear();
                    Console.WriteLine(function.RefundProduct());
                    Console.ReadLine();
                }
                else if (userOption == "4")
                {
                    Console.Clear();       
                    Console.WriteLine(function.FinishTransaction());
                    Console.ReadLine();
                    return;
                }
                else
                {
                    Console.WriteLine("\nSorry, that is not a valid option.  Please make a new selection.");
                    Console.ReadLine();
                }
                Console.Clear();      
            }
        }

        private void FeedMoney()
        {
            while (true)
            {
                Console.Clear();     
              
                Console.WriteLine("Insert dollar bill [takes 100s, 50s, 20s, 10s, 5s, 1s, .50s and .25s]\n");
                Console.WriteLine("   OR [X] when done adding money.\n");
                Console.WriteLine("Current money added: " + function.GetBalance() + "\n");
                string userResponse = Console.ReadLine().ToUpper();

                decimal result = function.FeedMoney(userResponse);

                if (result == -1)
                {
                    break;
                }
            }
        }

        private void SelectProduct()
        {
            Console.WriteLine("=========================================");
            Console.WriteLine();
  
            Console.WriteLine("Select an item to purchase. \n");
            Console.Write("  INPUT ITEM CODE  >>  ");
            string itemCode = Console.ReadLine().ToUpper();

            Console.WriteLine(function.SelectProduct(itemCode));
            Console.ReadLine();
        }
    }
}
