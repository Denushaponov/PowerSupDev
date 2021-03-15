using DevExpress.Mvvm;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfPaging.DistrictObjects;
using WpfPaging.Events;
using WpfPaging.Messages;
using WpfPaging.Pages;
using WpfPaging.Services;

namespace WpfPaging.ViewModels
{
    public class DistrictMenuViewModel:BindableBase
    {
        /// https://www.youtube.com/watch?v=9S5ATpelc8w
      
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;
        private readonly Repository _repository;


        /// <summary>
        /// Список микрорайонов
        /// </summary>
        public ObservableCollection<District> Districts { get; set; } = new ObservableCollection<District>();

        /// <summary>
        /// Выбранный микрорайон
        /// </summary>
       // private District _selectedDistrict = new District();
        public District SelectedDistrict { get; set; } 

      

       ExportData ExportPathInfo { get; set; }



        public DistrictMenuViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus, Repository repository)
        {

            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;
            _repository = repository;

            // Показ или не показ меню
           

            // Событие вызова метода сохранения м-района в репозитории
            _eventBus.Subscribe<OnSave<District>>(OnSaveDistrict);
            
            // Событие вызова метода удаления м-рна в репозитории
            _eventBus.Subscribe<OnDelete<District>>(OnDeleteDistrict);

            // Объявление пустого микрорайона
            Districts = new ObservableCollection<District>();

            // Обнаружение - есть ли сохранённые микрорайоны в текстовой базе данных
            repository.FindAll<District>().ContinueWith(s =>
              {
                  Districts = new ObservableCollection<District>(s.Result);
              }, TaskContinuationOptions.OnlyOnRanToCompletion);
           
            

            // Получаю микрорайон с изменениями от других моделей представления
            _messageBus.Receive<DistrictMessage>(this, async message =>
            {
                SelectedDistrict = message.SharedDistrict;
            });

            _messageBus.Receive<ExportPathMessage>(this, async message =>
            {
                ExportPathInfo = message.Export;
                ExportAsExcelHandler(ExportPathInfo.Dg, ExportPathInfo.CsvFileName, ExportPathInfo.ExcelFileName);
            });

            /// При получении измененного микрорайона = после нажатия пользователя на кнопку сохранить, срабатывает 
            ///событие которое вызывает метод сохранения выбранного микрорайона в репозитории
            _eventBus.Subscribe<SaveEvent>(async @event => SaveDistrict.Execute(SelectedDistrict));

       }

        /// <summary>
        /// Задание удаления микрорайона
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private Task OnDeleteDistrict(OnDelete<District> arg)
        {
            var item = Districts.FirstOrDefault(s => s.Id == arg.Id);
           if (item!=null)
            {
                Districts.Remove(item);
            }
            return Task.CompletedTask;
        }

        private  Task OnSaveDistrict(OnSave<District> arg)
        {
            byte i=0;
            //// !!!! здесь ловит гуид
            foreach (var d in Districts)
            {
                 
              if (arg.Id==d.Id)
              {
                    
                    Districts.Remove(d);
                    Districts.Insert(i, arg.Entity); 
                }
              i++;
            }
           
         
            return Task.CompletedTask;
        }

        public  ICommand RemoveDistrict => new AsyncCommand<District>(async(district) =>
        {
           await _repository.Remove<District>(district.Id);
           
        }, (district) => district != null);

        public ICommand AddDistrict => new DelegateCommand(() =>
        {
            Districts.Add(new District { Title = "Введіть назву" });
        });


        /// <summary>
        /// Команда сохраняет микрорайон по нажатию на кнопку
        /// </summary>        
        public ICommand SaveDistrict => new AsyncCommand(async() =>
        {
            if (SelectedDistrict==null)
            {
                MessageBox.Show("Оберіть мікрорайон для збереження");
                return;
            }
            MessageBox.Show("Мікрорайон " + SelectedDistrict.Title + " збережено");
            await _repository.Save(SelectedDistrict, SelectedDistrict.Id);
        });


        public ICommand GoForApartmentBuildings => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new Apartments());
            await _messageBus.SendTo<ApartmentsViewModel>(new DistrictMessage(SelectedDistrict));
            
        });

        public ICommand GoForCommercialBuildings => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new Commercials());
            await _messageBus.SendTo<CommercialsViewModel>(new DistrictMessage(SelectedDistrict));
            
        });



        public void ExportAsExcelHandler(DataGrid dg, string csvFileName, string excelFileName)
        {
            dg.SelectAllCells();
            dg.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            ApplicationCommands.Copy.Execute(null, dg);
            dg.UnselectAllCells();
            string result = (string)Clipboard.GetData(DataFormats.CommaSeparatedValue);
            if (File.Exists(csvFileName))
            {
                File.Delete(csvFileName);
            }
            
            File.AppendAllText(csvFileName, result, UnicodeEncoding.UTF8);
            if (File.Exists(excelFileName))
            {
                File.Delete(excelFileName);
            }
            string worksheetsName = "Сторінка 1";

            bool firstRowIsHeader = false;

            var format = new ExcelTextFormat();
            format.Delimiter = ',';
            format.EOL = "\r";              // DEFAULT IS "\r\n";
                                            // format.TextQualifier = '"';

            using (ExcelPackage package = new ExcelPackage(new FileInfo(excelFileName)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(worksheetsName);
                worksheet.Cells["A1"].LoadFromText(new FileInfo(csvFileName), format, OfficeOpenXml.Table.TableStyles.Medium27, firstRowIsHeader);
                package.Save();
            }
            File.Delete(csvFileName);
            MessageBox.Show("Таблицю " + excelFileName + " збережено");
        }





    }
}
