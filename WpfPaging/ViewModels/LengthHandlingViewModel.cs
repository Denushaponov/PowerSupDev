using DevExpress.Mvvm;
using DistrictSupplySolution.DistrictObjects;
using DistrictSupplySolution.Messages;
using DistrictSupplySolution.MessageWindows;
using DistrictSupplySolution.Pages;
using OfficeOpenXml;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfPaging.Events;
using WpfPaging.Messages;
using WpfPaging.Pages;
using WpfPaging.Services;

namespace DistrictSupplySolution.ViewModels
{
    public class LengthHandlingViewModel: BindableBase
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;

        /// <summary>
        /// Здесь хранится копия здания если нужно будет откатить значения назад к первоначальным
        /// </summary>
        private Substation _backupSelectedSubstation;
        public Substation SelectedSubstation { get; set; }
        public OptimizationDataBuilding SelectedOptiBuilding { get; set; }
        public LengthHandlingViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;

            _messageBus.Receive<SubstationMessage>(this, async message =>
            {
                SelectedSubstation = message.SharedSubstation;
                _backupSelectedSubstation = message.SharedSubstation;
            });
        }


        public ICommand SaveCommand => new AsyncCommand(async () =>
        {
            byte i = 0;
            foreach (var cm in SelectedSubstation.OptimizationDataBuildings)
            {
                 if (cm.CableLength == 0)
                    i++;
            }
            if (i != 0)
            { SelectedSubstation.IsLengthsCompleted = false; }
            else
            { SelectedSubstation.IsLengthsCompleted = true; }
            await _messageBus.SendTo<SubstationsViewModel>(new SubstationMessage(SelectedSubstation));

        });

        public ICommand CancelChanges => new AsyncCommand(async () =>
        {
            SelectedSubstation = _backupSelectedSubstation;
        });

        public ICommand LengthToExcel
        {
            get
            {
                return new AsyncCommand<DataGrid>(async (dg) =>
                {
                    ExportAsExcelHandler(dg, SelectedSubstation.Name);
                });
            }
        }
        public void ExportAsExcelHandler(DataGrid dg, string worksheetsName)
        {
            string directoryName = "Excel/Substations" + "/";
            string csvFileName = "CSV/" + "tempData.csv";
            string excelFileName = @"" + directoryName + SelectedSubstation.Name + ".xlsx";
            if (Directory.Exists("CSV") != true)
                Directory.CreateDirectory("CSV");
            if (Directory.Exists("Excel") != true)
                Directory.CreateDirectory("Excel");
            if (Directory.Exists(directoryName) != true)
            {
                Directory.CreateDirectory(directoryName);
            }


            dg.SelectAllCells();
            dg.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dg);
            dg.UnselectAllCells();
            string result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);

            if (File.Exists(csvFileName))
            {
                File.Delete(csvFileName);
            }


            File.AppendAllText(csvFileName, result, Encoding.UTF8);

            bool firstRowIsHeader = false;

            var format = new ExcelTextFormat();
            format.Delimiter = ',';
            format.EOL = "\r";              // DEFAULT IS "\r\n";
            format.TextQualifier = '"';

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFileName)))
            {
                try
                {
                    package.Workbook.Worksheets.Delete(worksheetsName);
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);
                    worksheet.Cells["A1"].LoadFromText(new FileInfo(csvFileName), format, OfficeOpenXml.Table.TableStyles.Medium27, firstRowIsHeader);
                    package.Save();
                }
                catch
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);
                    worksheet.Cells["A1"].LoadFromText(new FileInfo(csvFileName), format, OfficeOpenXml.Table.TableStyles.Medium27, firstRowIsHeader);
                    package.Save();
                }

            }
            File.Delete(csvFileName);
            MessageBox.Show("Таблицю" + excelFileName + " збережено");

        }
    }
}
