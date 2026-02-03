using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    //notes:
    //models will model the database to come
    // all contsraints, properties, data types, auto-generation, nullability, relationships  will be defined here as per the database design

    //Recommended Annotations to use:
    //mopdels mus tbe named in singular form and pascal case eg ProductName, Category , CustomerOrder
    public class Product
    {
        public Product()
        { Stock = 1; }

        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int Id { get; set; }

        [StringLength(200)]
        [Required]
        public  string  Name { get; set; }

        [ForeignKey("Category")] //we link the two properties so at runtime; category (navigation property) is linked to CategoryFK (foreign key property). this is the lazy loading package at work
        public int  CategoryFK { get; set; }

        public virtual required Category Category { get; set; }  //i.e  myProduct.Category.CategoryName navigational property

        public double Price { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public int Stock { get; set; }

        public string? ImagePath { get; set; }

    }
}
