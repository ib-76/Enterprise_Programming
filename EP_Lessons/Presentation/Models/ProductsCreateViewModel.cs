using Common.Models;

namespace Presentation.Models
{

    //view models  are used top present on screen a selection of DB data
    //are also a way how to transport data from the controller to the view or vice-versa

    //Data1= Product
    //DAta2= List<Category>
    //DATA3= List<Status>.... another exampe..
    public class ProductsCreateViewModel
    {
        public Product Product {  get; set; }
        public List<Category> Categories { get; set; }

       
    }
}
