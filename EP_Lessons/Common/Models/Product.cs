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


    //Validators:
    //Required, StringLength, Range, Compare , RegularExpression
    public class Product
    {

        //[Compare("ConfirmPassword", ErrorMessage ="" )]
        //public string Password { get; set; }
        //public string ConfirmPassword { get; set; }

        public Product()
        { Stock = 1; }

        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
       public int Id { get; set; }


        [StringLength(250, ErrorMessage = "Cannot exceed 250 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Product name is empty")]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "Product names must be made up of letters, numbers and spaces")]
        public  string  Name { get; set; }

        [ForeignKey("Category")] //we link the two properties so at runtime; category (navigation property) is linked to CategoryFK (foreign key property). this is the lazy loading package at work
        public int  CategoryFK { get; set; }

        public virtual required Category Category { get; set; }  //i.e  myProduct.Category.CategoryName navigational property


        [Required(AllowEmptyStrings = false, ErrorMessage = "Product price is empty")]
        [Range(0, 100000, ErrorMessage = "Prices may vary between 0 and 100000. if more contact admin@gmail.com")]
        public double Price { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }


        [Range(0, 100000, ErrorMessage = "Stock amount may vary between 0 and 1000000. if more contact admin@gmail.com")]
        public int Stock { get; set; }

        public string? ImagePath { get; set; }

        public bool Discount { get; set; }

    }
}
