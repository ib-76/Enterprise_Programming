using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    internal class OrderItem
    {


        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductFK { get; set; }
        public virtual Product Product { get; set; } //navigation property , virtual is used for lazy loading to function properly
        public int Quntity { get; set; }

        public double Price { get; set; }

        [ForeignKey("Order")]
        public Guid OrderFK { get; set; }
        public virtual Order Order { get; set; } //navigation property to access the order    
    }
}
