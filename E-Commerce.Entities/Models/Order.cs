using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.Models
{
    public class Order
    {
        public int Id { get; set; }
        [ValidateNever]
        public List<ShoppingCart> ShoppingCarts { get; set; } // Corrected capitalization
        [ValidateNever]
        public decimal TotalPrice { get; set; }
        [ValidateNever]
        public String UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicatonUser User { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }

        public string? TrakcingNumber { get; set; }
        public string? Carrier { get; set; }

        public DateTime PaymentDate { get; set; }

        //Stripe Properties

        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ShippingDate { get; set; }
    }
}
