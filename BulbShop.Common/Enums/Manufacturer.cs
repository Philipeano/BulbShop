using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulbShop.Common.Enums
{
    public enum Manufacturer
    {
        Unknown = 0,
        Apple,
        Samsung,

        [Description("Nigerian Breweries")]
        NigerianBreweries,

        Guinness,

        [Description("Nigerian Bottling Company")]
        NigerianBottlingCompany,
        Cadburys,

        NestleFoods,
        Sony,
        Scanfrost,
        LG,
        Dell,

        [Description("Hewlett Packard")]
        HewlettPackard
    }
}
