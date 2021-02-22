using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace WpfPaging.Messages
{
    class ExportPathMessage:IMessage
    {
        // Сообщение передаёт коллекцию
        public ExportPathMessage(ExportData export)
        {
            Export = export;
        }

        
       public ExportData Export { get; set; }
      
    }
    

    public class ExportData 
    {
        public DataGrid Dg { get; set; }
        public string CsvFileName { get; set; }
        public string ExcelFileName { get; set; }
    }
}
