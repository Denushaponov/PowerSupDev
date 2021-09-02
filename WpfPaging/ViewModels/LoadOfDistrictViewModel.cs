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

        District _selectedDistrict = new District();
        public District SelectedDistrict
        {
            get { return _selectedDistrict; }
            set 
            {
                if (_selectedDistrict.Building == value.Building)
                    _isDistrictUpdated = false;
                else
                    _isDistrictUpdated = true;
                _selectedDistrict = value;
                
            }
        } 
        public Street SelectedStreet { get; set; }
        public AbstractBuilding SelectedAbstractBuilding { get; set; }

        bool _isDistrictUpdated = false;
          
        public LoadOfDistrictViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;

           

            _messageBus.Receive<DistrictMessage>(this, async message =>
            {
                // присваивание присланного из DistrictMenuVM микрорайона в качестве выбранного
                SelectedDistrict = message.SharedDistrict;

                // валидация возможности рассчёта  нагрузки
                if (SelectedDistrict.IsReadyToDetermineCOP== false)
                {
                    SelectedDistrict.IsReadyForCalculation = false;
                }


            });

            // получение абстрактного здания с отредактироваными значениями
            _messageBus.Receive<AbstractBuildingMessage>(this, async message => 
            {
                
                int i = 0;
                foreach (var ab in SelectedDistrict.AbstractBuildings)
                {
                    if (ab.Id==message.SharedAbstractBuilding.Id)
                    {
                        break;
                    }
                    else
                    i++;
                }

                SelectedDistrict.AbstractBuildings[i] = message.SharedAbstractBuilding;



                // Проверяю можно ли давать пользователю доступ к кнопке определение коэффициентов участия в максимуме
                i = 0;
                foreach (var ab in SelectedDistrict.AbstractBuildings)
                {
                    if (ab.SideNote=="Особливий")
                    {
                        i++;  
                    }    
                }
               if (i == 0)
                { 
                    SelectedDistrict.IsReadyToDetermineCOP = true; 
                
                }
               else
                { SelectedDistrict.IsReadyToDetermineCOP = false; }
               // Если пользователь сделал изменение и отправляет его в форму для рассчёта то он не может сразу начать рассчёт микрорайона а сначала
               // должен определить , нажать кнопку определить коэффициенты участия в максимуме.
                SelectedDistrict.IsReadyForCalculation = false;
            
            });

            SelectedDistrict.Streets = new ObservableCollection<Street> {};
            
            SelectedDistrict.AbstractBuildings = new ObservableCollection<AbstractBuilding> { };
           
        }

        // Начало рассчёта
        public ICommand SendAbstractBuildings => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<DistrictMenuViewModel>(new DistrictMessage(SelectedDistrict));
            await _eventBus.Publish(new SaveEvent());
        });

        public ICommand ConvertBuildingsToAbstractBuildingsCommand => new DelegateCommand(()=>
        {
            if (_isDistrictUpdated == true)
            {
                SelectedDistrict.ConvertBuildingsToAbstractBuildings();
                _isDistrictUpdated = false;
            }
        });
       

        public ICommand DetermineCoefficientsOfParticipanceInMaximumLoadCommand => new DelegateCommand(() =>
        {
            SelectedDistrict.DetermineCoefficientsOfParticipanceInMaximumLoad();
            // Пользователь определил коэффициенты участия в максимуме, теперь можно дать доступ к рассчёту
            SelectedDistrict.IsReadyForCalculation = true;
        });

        public ICommand CalculateDistrictPower => new DelegateCommand(() =>
        {
           
            SelectedDistrict.CalculateDistrictPower();
        });

        
        

        

        public ICommand CalculateLigtningCommand => new DelegateCommand(() =>
        {
            SelectedDistrict.CalculateLightning();
            
            // Проверяю также возможность доступа пользователя к определению коэффициентов участия
            byte i = 0;
            foreach (var ab in SelectedDistrict.AbstractBuildings)
            {
                if (ab.SideNote == "Особливий")
                {
                    i++;
                }
            }
            if (i == 0)
            { SelectedDistrict.IsReadyToDetermineCOP = true; }
            else
            { SelectedDistrict.IsReadyToDetermineCOP = false; }
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
