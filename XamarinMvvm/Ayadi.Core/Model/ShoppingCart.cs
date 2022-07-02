using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class ShoppingCart : BaseModel
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
        public string Shopping_cart_type { get; set; }
        public int Product_id { get; set; }
        public Product Product { get; set; }
        public int Customer_id { get; set; }

        //public User Customer { get; set; }

        public ShoppingCart()
        {
            Shopping_cart_type = "ShoppingCart";
            //Shopping_cart_type = "Wishlist";
        }

        public override bool Equals(object obj)
        {
            try
            {
                if (obj is ShoppingCart)
                {
                    ShoppingCart outCart = obj as ShoppingCart;
                    return outCart.Id == this.Id;
                }
                return base.Equals(obj);
            }
            catch 
            {
                return base.Equals(obj);
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
