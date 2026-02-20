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
    public class MenuItem : ItemValidating
    {
        [Key]
        public Guid Id { get; set; }

        public string BistrobaseId { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
        public bool? Status { get; set; }
      

        // Foreign key
        [ForeignKey("RestaurantId")]             
        public virtual Restaurant Restaurant { get; set; } // Navigation property
        public int RestaurantId { get; set; }

        public List<string> GetValidators()
        {
            // for now, just a hardcoded example
            return new List<string> { "joecardona@goldenspoon.com" };
        }

        public string GetCardPartial()
        {
            return "_MenuItemCard.cshtml";
        }



    }
}
