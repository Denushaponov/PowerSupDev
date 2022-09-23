using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistrictSupplySolution
{
    public class FinalReport : BindableBase
    {
        public string SubstationName { get; set; }
        public string Buildings { get; set; }
        public string Load { get; set; }
        public string CoeficientOfLoad { get; set; }
        public string LengthsOfCable { get; set; }
       
    }
}
