using DevExpress.Mvvm;
using System;
using System.Windows.Input;
using WpfPaging.Events;
using WpfPaging.Messages;
using WpfPaging.Pages;
using WpfPaging.Services;
using WpfPaging.DistrictObjects;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.IO;
using System.Windows;
using System.Text;
using OfficeOpenXml;

namespace WpfPaging.ViewModels
{
    public class CommercialsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;

     
        /// <summary>
        /// Микрорайон который присылается, редактируется и отсылается
        /// </summary>
        public District SelectedDistrict { get; set; } = new District();

        /// <summary>
        /// Выбранное коммерческое здание, которое редактируется
        /// </summary>
        public CommercialBuilding SelectedCommercialBuilding { get; set; }

        public CommercialsViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;

            SelectedDistrict.Building.CommercialBuildings= new ObservableCollection<CommercialBuilding> { };

            _messageBus.Receive<DistrictMessage>(this, async message =>
            {
                SelectedDistrict = message.SharedDistrict;
            });



        }

        /// <summary>
        /// Добавление нового коммерческого здания
        /// </summary>
        public ICommand AddCommand => new AsyncCommand(async () =>
        {
            CommercialBuilding commercialBuilding = new CommercialBuilding();
            SelectedDistrict.Building.CommercialBuildings.Insert(0, commercialBuilding);
            SelectedCommercialBuilding = commercialBuilding;
        }
     );

        /// <summary>
        /// Удаление выбраноого коммерческого здания
        /// </summary>
        public ICommand Remove
        {
            get
            {
                return new DelegateCommand<CommercialBuilding>((commercialBuilding) =>
                {
                    SelectedDistrict.Building.CommercialBuildings.Remove(commercialBuilding);

                }, (commercialBuilding) => commercialBuilding != null);
            }
        }

        /// <summary>
        /// Отправка данных в главное меню для сохранения и сохранение
        /// </summary>
        public ICommand SendCommercialBuildings => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<DistrictMenuViewModel>(new DistrictMessage(SelectedDistrict));
            await _eventBus.Publish(new SaveEvent());
        });

        public ICommand ExecuteCalculation => new AsyncCommand(async () =>
        {
            SelectedDistrict.CalculateCommercialBuildings();
            await _messageBus.SendTo<DistrictMenuViewModel>(new DistrictMessage(SelectedDistrict));
            await _eventBus.Publish(new SaveEvent());
        }
       );

        public ICommand InitialCommercialBuildingsDataToExcel
        {
            get
            {
                return new AsyncCommand<DataGrid>(async (dg) =>
                {
                    
                    ExportAsExcelHandler(dg, "Вхідні дані");
                });
            }
        }

        public ICommand CalculatedCommercialBuildingsDataToExcel
        {
            get
            {
                return new AsyncCommand<DataGrid>(async (dg) =>
                {
                    
                    ExportAsExcelHandler(dg, "Результат розрахунків");
                });
            }
        }



        public void ExportAsExcelHandler(DataGrid dg, string worksheetsName)
        {
            string directoryName = "Excel/" + SelectedDistrict.Title + "/";
            string csvFileName = "CSV/" + "tempData.csv";
            string excelFileName = @"" + directoryName + "Комерційні_будівлі_мікрорайону" + ".xlsx";
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
