using DistrictSupplySolution.DistrictObjects;
using System;
using System.Collections.Generic;
using System.Text;
using WpfPaging.Messages;

namespace DistrictSupplySolution.Messages
{
    class AbstractBuildingMessage:IMessage
    {
        public AbstractBuildingMessage(AbstractBuilding sharedAbstractBuilding)
        {
            SharedAbstractBuilding = sharedAbstractBuilding;
        }

        public AbstractBuilding SharedAbstractBuilding { get; set; }
    }
}
