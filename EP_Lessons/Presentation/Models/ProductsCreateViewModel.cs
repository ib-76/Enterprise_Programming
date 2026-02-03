using Common.Models;

namespace Presentation.Models
{

    //view models  are used top present on screen a selection of DB data
    //are also a way how to transport data from the controller to the view or vice-versa

    //Data 1 => Product
    //Data 2 => List<Category>
    //Data 3 => List<Status> //we don't have this
    public class ProductsCreateViewModel
    {
        public Product Product {  get; set; }
        public List<Category> Categories { get; set; }

        // I formfile is a datatype to handle files 
        public IFormFile ImageFile { get; set; }

       
    }
}
