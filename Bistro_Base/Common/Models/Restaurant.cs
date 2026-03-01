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
    public class Restaurant : IitemValidating
    {
        [Key]
        public Guid Id { get; set; }

        public string BistrobaseId { get; set; }

        public string Name { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }
        public string OwnerEmailAddress { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool? Status { get; set; }
        public string ImagePath { get; set; }

        // Navigation property
        public virtual ICollection<MenuItem> MenuItems { get; set; }


        public ItemType GetCardPartial()
        {
            return ItemType.Restaurant;
        }

        public List<string> GetValidators()
        {
             return new List<string> {  "luca.owner@example.com", "hana.owner@example.com" };
        }
    }
}
