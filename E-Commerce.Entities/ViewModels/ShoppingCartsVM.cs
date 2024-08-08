using E_Commerce.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.ViewModels
{
    public class ShoppingCartsVM
    {
        public IEnumerable<ShoppingCart> shoppingCarts { get; set; }
        public Decimal ToatalPrice { get; set; }
        public int TotalItems { get; set; }

    }
}
