using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.Models
{
    public class ApplicatonUser : IdentityUser
    {
        [Required]
        public String Name { get; set; }
        public String City { get; set; }
        public String Address { get; set; }
        public List<ShoppingCart> shoppingCarts { get; set; }
        public List<Order> Orders { get; set; }


    }
}
