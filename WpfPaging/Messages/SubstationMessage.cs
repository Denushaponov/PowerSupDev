using DistrictSupplySolution.DistrictObjects;
using System;
using System.Collections.Generic;
using System.Text;
using WpfPaging.DistrictObjects;
using WpfPaging.Messages;

namespace DistrictSupplySolution.Messages
{
    class SubstationMessage: IMessage
    {
        public SubstationMessage(Substation sharedSubstation)
        {
            SharedSubstation = sharedSubstation;
        }

        public Substation SharedSubstation { get; set; }
    }
}
