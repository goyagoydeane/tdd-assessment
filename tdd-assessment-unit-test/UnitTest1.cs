using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tdd_assessment;

namespace tdd_assessment_unit_test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Function_FeedMoney()
        {
            Function function = new Function(new Dictionary<string, Stock>());

            Assert.AreEqual(100M, function.FeedMoney("100"));
            function = new Function(new Dictionary<string, Stock>());       
            Assert.AreEqual(50M, function.FeedMoney("50"));
            function = new Function(new Dictionary<string, Stock>());
            Assert.AreEqual(20M, function.FeedMoney("20"));
            function = new Function(new Dictionary<string, Stock>());
            Assert.AreEqual(0.50M, function.FeedMoney(".50"));
        }


        [TestMethod]
        public void Function_SelectProduct_InsufficentFunds()
        {
            Menu menu = new Menu();
            Function function = new Function(menu.RestockItems());

            //StockID = 1
            //ProductName = Coke
            //Price = $25.00

            string StockID = "1";

            string expectedOutput = $"\nInsufficient funds. \nPlease insert additional $25.00 to purchase this selection.";
            Assert.AreEqual(expectedOutput, function.SelectProduct(StockID));
        }

        [TestMethod]
        public void Function_SelectProduct_Dispense()
        {
            //StockID = 1
            //ProductName = Coke
            //Price = $25.00

            string StockID = "1";

            Menu menu = new Menu();
            Dictionary<string, Stock> stocks = menu.RestockItems();
            Function function = new Function(stocks);
           
            function.FeedMoney("100");
            string expectedOutput = String.Concat($"\nDispensing { stocks[StockID].StockItemType.ProductName}\n",
                $"ITEM PRICE: ${stocks[StockID].StockItemType.Price}\n", "Remaining balance is " + String.Format("{0:C}", 75));

            Assert.AreEqual(expectedOutput, function.Dispense(StockID));
        }


        [TestMethod]
        public void Function_SelectItem_InvalidItemCode()
        {
            string StockID = "7"; // not existed

            Menu menu = new Menu();
            Dictionary<string, Stock> stocks = menu.RestockItems();
            Function function = new Function(stocks);

            function.FeedMoney("100");

            string expectedOutput = "\nINVALID ITEM CODE \nReturning to purchase menu. . . .";
            Assert.AreEqual(expectedOutput, function.SelectProduct("g13"));
        }

        [TestMethod]
        public void Function_SelectProduct_Refund()
        {
            //StockID = 1
            //ProductName = Coke
            //Price = $25.00

            string StockID = "1";

            Menu menu = new Menu();
            Dictionary<string, Stock> stocks = menu.RestockItems();
            Function function = new Function(stocks);

            function.FeedMoney("100");
            string expectedOutput = String.Concat($"Refunding { stocks[StockID].StockItemType.ProductName}\n",
                $"ITEM PRICE: ${stocks[StockID].StockItemType.Price}\n", $"Remaining balance is { String.Format("{0:C}", 100)}\n\n");

            function.Dispense(StockID);

            Assert.AreEqual(expectedOutput, function.RefundProduct());
        }
    }
}
