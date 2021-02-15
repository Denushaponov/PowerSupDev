using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WpfPaging.DistrictObjects
{
    public class District:BindableBase
    {
        public string Title { get; set; }
        public Building Building { get; set; }
        
    }
}
