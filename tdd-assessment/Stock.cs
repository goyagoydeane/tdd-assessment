using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdd_assessment
{
   public class Stock
    {
        private List<Item> itemsInStock;
        private string stockId;
        private Item stockItemType;

        public List<Item> ItemsInStock
        {
            get { return this.itemsInStock; }
        }

        public string SlotID
        {
            get { return this.stockId; }
        }

        public Item StockItemType
        {
            get { return this.stockItemType; }
        }

        public int NumItemsInStock
        {
            get { return itemsInStock.Count; }
        }

        public bool IsEmpty
        {
            get { return this.itemsInStock.Count == 0; }
        }

        public Stock(string stockId, List<Item> itemsInSlot)
        {
            this.stockId = stockId;
            this.itemsInStock = itemsInSlot;
            this.stockItemType = itemsInSlot[0];
        }

        public Item RemoveProduct()
        {
            Item p = this.itemsInStock[0];
            this.itemsInStock.RemoveAt(0);
            return p;
        }

        public string DisplayItemsInSlot()
        {
            int colWidth = 10;

            if (this.NumItemsInStock > 0)
            {
                return ($"{this.stockId} |".PadRight(colWidth / 2) + $" {this.stockItemType.ProductName}".PadRight(colWidth * 2) +
                    $" | ${this.stockItemType.Price} |".PadRight(colWidth) + $" Qty: {this.NumItemsInStock}");
            }
            else
            {
                return ($"{this.stockId} |".PadRight(colWidth / 2) + $"{this.stockItemType.ProductName}".PadRight(colWidth * 2) +
                    $"|{this.stockItemType.Price} |".PadRight(colWidth) + " SOLD OUT");
            }
        }
    }
}
