using System;
using System.Collections.Generic;
using System.Text;
using WpfPaging.DistrictObjects;

namespace WpfPaging.Messages
{
    class DistrictMessage:IMessage
    {
        // Сообщение передаёт коллекцию
        public DistrictMessage(District sharedDistrict)
        {
            SharedDistrict = sharedDistrict;
        }

        public District SharedDistrict { get; set; }
    }
}
