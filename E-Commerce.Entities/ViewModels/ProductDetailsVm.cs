using E_Commerce.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.ViewModels
{
    public class ProductDetailsVm
    {
        public Product Product { get; set; }
        public int ProductCount { get; set; }
    }
}
