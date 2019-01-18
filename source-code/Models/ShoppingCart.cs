using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ShoppingCart
    {
        public string paid_key { get; set; }
        public List<CartItem> cartItem = new List<CartItem>();
        public float taxTotal { get; set; }
        public float CartTotal { get; set; }

        public float PromotionTotal { get; set; }
        public promotion promotion { get; set; }
        public ShoppingCart()
        {
            taxTotal=0;
            CartTotal = 0;
            PromotionTotal = 0;
        }

    }
    public class CartItem
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int Qty { get; set; }
        public float LineTotal { get; set; }
        public float Tax { get; set; }
        public string StockName { get; set; }
        public int StockId{ get; set; }
        public int StockMaxValue { get; set; }
        public CartItem()
        {
            Code = "";
            Name = "";
            Price = 0;
            Qty = 0;
            LineTotal = 0;
            Tax = 0;
        }
    }
}