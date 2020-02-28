using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdd_assessment
{
    public class Function
    {
        static List<Item> purchasedItems = new List<Item>();
        static List<string> auditEntriesToWrite = new List<string>();
        static Dictionary<string, int> salesReportToWrite = new Dictionary<string, int>();
        decimal currentMoneyProvided = 0.0m;
        static decimal totalSales = 0.0m;


        private Dictionary<string, Stock> Stocks { get; }

        public Function(Dictionary<string, Stock> stocks)
        {
            this.Stocks = stocks;
            InitializeSalesReport();
        }

        public void InitializeSalesReport()
        {
            foreach (KeyValuePair<string, Stock> kvp in Stocks)
            {
                string key = kvp.Value.StockItemType.ProductName;
                salesReportToWrite[key] = 0;
            }
        }

        public List<string> ListAllItems()
        {
            List<string> itemsToPrint = new List<string>();
            foreach (KeyValuePair<string, Stock> kvp in Stocks)
            {
                itemsToPrint.Add(kvp.Value.DisplayItemsInSlot());
            }
            return itemsToPrint;
        }

        public decimal FeedMoney(string userResponse)
        {
            decimal previousBalance = currentMoneyProvided;
            
            switch (userResponse)
            {
                case ".25":
                    currentMoneyProvided += 0.25m;
                    break;
                case ".50":
                    currentMoneyProvided += 0.50m;
                    break;
                case "1":
                    currentMoneyProvided += 1;
                    break;
                case "5":
                    currentMoneyProvided += 5;
                    break;
                case "10":
                    currentMoneyProvided += 10;
                    break;
                case "20":
                    currentMoneyProvided += 20;
                    break;
                case "50":
                    currentMoneyProvided += 50;
                    break;
                case "100":
                    currentMoneyProvided += 100;
                    break;
                case "X":
                    return -1m;
                default:
                    Console.WriteLine("Bill not accepted.");
                    break;
            }
           
            return currentMoneyProvided;
        }

        public string SelectProduct(string itemCode)
        {
            if (!Stocks.ContainsKey(itemCode))
            {
                return "\nINVALID ITEM CODE \nReturning to purchase menu. . . .";
            }
            else if (Stocks[itemCode].IsEmpty)
            {
                return "\nITEM IS SOLD OUT \nReturning to purchase menu. . . .";
            }
            else if (currentMoneyProvided < Stocks[itemCode].StockItemType.Price)
            {
                string moneyStillNeeded = String.Format("{0:C}", Stocks[itemCode].StockItemType.Price - currentMoneyProvided);
                return $"\nInsufficient funds. \nPlease insert additional {moneyStillNeeded} to purchase this selection.";
            }        
            else
            {
                return Dispense(itemCode);
            }
        }

        public string GetBalance()
        {
            return String.Format("{0:C}", currentMoneyProvided);
        }

        public string Dispense(string itemCode)
        {
            decimal previousBalance = currentMoneyProvided;
            currentMoneyProvided -= Stocks[itemCode].StockItemType.Price;
            
            UpdateAuditReport(previousBalance, "DISPENSE", itemCode);
            
            purchasedItems.Add(Stocks[itemCode].RemoveProduct());
            
            salesReportToWrite[Stocks[itemCode].StockItemType.ProductName]++;
            totalSales += Stocks[itemCode].StockItemType.Price;

            string output = String.Concat($"\nDispensing {Stocks[itemCode].StockItemType.ProductName}\n",
                $"ITEM PRICE: ${Stocks[itemCode].StockItemType.Price}\n", "Remaining balance is " + String.Format("{0:C}", currentMoneyProvided));
            return output;
        }

        public string FinishTransaction()
        {
            decimal previousBalance = currentMoneyProvided;
            
            string outputMessage = CalculateChange();
            
            currentMoneyProvided = 0.0m;
            
            UpdateAuditReport(previousBalance, "FINISH");
            
            outputMessage = String.Concat(outputMessage, ConsumePurchases());

            return outputMessage;
        }

        private string ConsumePurchases()
        {
            string consumptionSounds = "";
            
            foreach (Item purchase in purchasedItems)
            {
                consumptionSounds += $"\nExcited to enjoy my {purchase.ProductName}! ";
            }
            return consumptionSounds;
        }

        public string RefundProduct() {
            string[] itemIds = purchasedItems.Select(x => x.ItemId).Distinct().ToArray();

            decimal totalRefund = 0;
            string output = string.Empty;

            foreach (var itemId in itemIds) {
                Stocks[itemId].ItemsInStock.AddRange(purchasedItems.Where(x => x.ItemId == itemId));
                totalRefund += purchasedItems.Where(x => x.ItemId == itemId).Sum(x => x.Price);
                purchasedItems.Remove(purchasedItems.Where(x => x.ItemId == itemId).FirstOrDefault());

                output += String.Concat($"Refunding {Stocks[itemId].StockItemType.ProductName}\n",
               $"ITEM PRICE: ${Stocks[itemId].StockItemType.Price}\n", $"Remaining balance is { String.Format("{0:C}", currentMoneyProvided + totalRefund)}\n\n");
            }

            currentMoneyProvided += totalRefund;
           
            return output;
        }

        private string CalculateChange()
        {
            int currentBalanceInCents = (int)(currentMoneyProvided * 100);
            int numQuartersBack = currentBalanceInCents / 25;
            int numDimesBack = (currentBalanceInCents - 25 * numQuartersBack) / 10;
            int numNickelsBack = (currentBalanceInCents - 25 * numQuartersBack - 10 * numDimesBack) / 5;
            
            string changeReceived = $"\nCHANGE RECEIVED BACK: {numQuartersBack} quarter(s), {numDimesBack} dime(s)," +
                $"{numNickelsBack} nickel(s)\n \t for a total of " + String.Format("{0:C}", currentMoneyProvided) + " received back.\n";
            return changeReceived;
        }

        private void UpdateAuditReport(decimal startingAmount, string keyword, string slotID)
        {
            string auditEntry = "";
            int colWidth = 15;
            if (keyword == "DISPENSE")
            {
                auditEntry = String.Concat(DateTime.Now, " ",
                    $"{Stocks[slotID].StockItemType.ProductName} {Stocks[slotID].StockItemType.ItemId}".PadRight((colWidth * 2) - 1),
                    String.Format("{0:C}", startingAmount).PadRight((colWidth - 6)),
                    $"{String.Format("{0:C}", currentMoneyProvided)}");
            }
            auditEntriesToWrite.Add(auditEntry);
        }

        private void UpdateAuditReport(decimal startingAmount, string keyword)
        {
            string auditEntry = "";
            int colWidth = 15;

            if (keyword == "FEEDMONEY")
            {
                auditEntry = String.Concat(DateTime.Now, " FEED MONEY:".PadRight(colWidth * 2),
                String.Format("{0:C}", currentMoneyProvided - startingAmount).PadRight((colWidth / 2) + 2),
                $"{String.Format("{0:C}", currentMoneyProvided)}");
            }
            else if (keyword == "FINISH")
            {
                auditEntry = String.Concat(DateTime.Now, " GIVE CHANGE:".PadRight(colWidth * 2),
                    String.Format("{0:C}", startingAmount).PadRight((colWidth / 2) + 2),
                    $"{String.Format("{0:C}", currentMoneyProvided)}");
            }
            auditEntriesToWrite.Add(auditEntry);
        }

        public void ShowProducts(List<string> itemsToPrint)
        {
            foreach (string itemInfo in itemsToPrint)
            {
                Console.WriteLine(itemInfo);
            }
        }
    }
}
