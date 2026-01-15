using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Order
    {
        //8CGUJKIOJF-7YY9-H4X2-A5D6-3K5G7H8I9J0L guid property

        [Key]
        public Guid Id { get; set; }

        public string Username { get; set; }

        public DateTime DatePlaced { get; set; }

        public double FinalPrice { get; set; } 

        public IQueryable<OrderItem> OrderItems { get; set; } //navigation property to access all order items in this order

    }
}
