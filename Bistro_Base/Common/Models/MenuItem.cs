using Common.Enums;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class MenuItem : IitemValidating
    {
        [Key]
        public Guid Id { get; set; }
        public string BistrobaseId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
        public bool? Status { get; set; }

        public string ImagePath { get; set; }
        // Foreign key
        public Guid RestaurantId { get; set; }

        [ForeignKey("RestaurantId")]             
        public virtual required Restaurant Restaurant { get; set; } // Navigation property



        public ItemType GetCardPartial()
        {
            return ItemType.MenuItem;
        }

        public List<string> GetValidators()
        {
            return new List<string> { "admin1@site.com", "admin2@site.com" };
        }
    }
}
