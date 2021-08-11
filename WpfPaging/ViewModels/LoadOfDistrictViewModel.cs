using DevExpress.Mvvm;
using DistrictSupplySolution.DistrictObjects;
using DistrictSupplySolution.Messages;
using DistrictSupplySolution.Pages;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfPaging.DistrictObjects;
using WpfPaging.Events;
using WpfPaging.Messages;
using WpfPaging.Services;
using WpfPaging.ViewModels;

namespace DistrictSupplySolution.ViewModels
{
    public class LoadOfDistrictViewModel:BindableBase
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;

        public District SelectedDistrict { get; set; } = new District();
        public Street SelectedStreet { get; set; }
        public AbstractBuilding SelectedAbstractBuilding { get; set; }

      
        public bool CalculationButtonIsEnabled=false; 
          
        public LoadOfDistrictViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;

           

            _messageBus.Receive<DistrictMessage>(this, async message =>
            {
                // присваивание присланного из DistrictMenuVM микрорайона в качестве выбранного
                SelectedDistrict = message.SharedDistrict;
            });
            SelectedDistrict.Streets = new ObservableCollection<Street> { new Street { Category = "A" }, new Street { Category = "B" }, new Street { Category = "C" } };
            
            SelectedDistrict.AbstractBuildings = new ObservableCollection<AbstractBuilding> { };
           
        }

        // Начало рассчёта
        public ICommand SendAbstractBuildings => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<DistrictMenuViewModel>(new DistrictMessage(SelectedDistrict));
            await _eventBus.Publish(new SaveEvent());
        });

        public ICommand CalculateDistrict => new DelegateCommand(() =>
        {
            SelectedDistrict.DetermineCoefficientsOfParticipanceInMaximumLoad();
        });

        public ICommand CalculateDistrictPower => new DelegateCommand(() =>
        {
            SelectedDistrict.CalculateDistrictPower();
        });

        public ICommand CalculateLigtningCommand => new DelegateCommand(() =>
        {
            SelectedDistrict.CalculateLightning();
            CalculationButtonIsEnabled = true;
        });


        public ICommand ClearCommand => new DelegateCommand(() =>
        {
            SelectedDistrict.AbstractBuildings.Clear();
        });


        /// <summary>
        /// Отправить выбранное абстрактное здание для редактирования в его вьюмодель
        /// перейти на интерфейчсу редактирования
        /// </summary>
        public ICommand UserWantsToAddCustomCoefficientsOfMax => new AsyncCommand(async () =>
        {
            // переход на страницу где буду редактьировать коєфициентіф учасития в максимуме

            _pageService.ChangePage(new CustomCoefficientsOfMaxPage());
            await _messageBus.SendTo<AbstractBuildingViewModel>(new AbstractBuildingMessage(SelectedAbstractBuilding));

        });


        public ICommand CoefficientsOfParticipanceDataToExcel
        {
            get
            {
                return new AsyncCommand<DataGrid>(async (dg) =>
                {

                    ExportAsExcelHandler(dg, "Коефіцієнти участі в максимумі навантаження");
                });
            }
        }



        public void ExportAsExcelHandler(DataGrid dg, string worksheetsName)
        {
            string directoryName = "Excel/" + SelectedDistrict.Title + "/";
            string csvFileName = "CSV/" + "tempData.csv";
            string excelFileName = @"" + directoryName + "_коефіціенти_участі_у_максимумі_навантаження_мікрорайону" + ".xlsx";
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
