using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Common.Enums
{
    public enum OrderStatus
    {
        Unknown = 0,
        Pending,  
        Cancelled,
        Confirmed,
        Processing,
        Completed
    }
}
