using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface IItemsRepository
    {
        List<IitemValidating> Get();
        void Save(List<IitemValidating> items);


    }
}
