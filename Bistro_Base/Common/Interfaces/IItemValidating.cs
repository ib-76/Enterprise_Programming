using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Enums;


namespace Common.Interfaces
{
    public interface IitemValidating
    {
        string BistrobaseId { get; }

        List<string> GetValidators();
        ItemType GetCardPartial();
    }
}
