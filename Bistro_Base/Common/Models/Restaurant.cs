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
    public class Restaurant : ItemValidating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string BistrobaseId { get; set; }

        public string Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }
        public string OwnerEmailAddress { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool? Status { get; set; }

        // Navigation property
        public virtual ICollection<MenuItem> MenuItems { get; set; }


        public List<string> GetValidators()
        {
            // hardcoded admin email for now
            return new List<string> { "admin@restautants.com" };
        }

        public string GetCardPartial()
        {
            return "_RestaurantCard.cshtml";
        }
    }
}
