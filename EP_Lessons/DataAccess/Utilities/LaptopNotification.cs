using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Utilities
{
    public class LaptopNotification : INotification
    {
        public void Notify(string message)
        {
            using (var sw = System.IO.File.AppendText(@"G:\Enterprise Programming\Enterpise_Programming\EP_Lessons\Presentation\laptops.log"))
            {
                sw.WriteLine(message);
            }
        }
    }
}