using DevExpress.Mvvm;
using DistrictSupplySolution.DistrictObjects;
using DistrictSupplySolution.Messages;
using DistrictSupplySolution.Pages;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfPaging.DistrictObjects;
using WpfPaging.Events;
using WpfPaging.Messages;
using WpfPaging.Services;
using WpfPaging.ViewModels;

namespace DistrictSupplySolution.ViewModels
{
    #region
    /*
     Цель: Выводить цевт ячейки - красный или зелёный в зависимости 
    от того все ли длины данной ТП != 0, или если добавляется новое
    здание его длина = 0

    Нужно ли сравнивать со староц коллекцией? (нет, потомку что проверка
    =0 идёт
    где поводить проверку на 0

    Если происходит изменение данных в коллекции Длин,  
    То производить проверку на 0
    Если есть 0 - то запретить дальнейший рассчёт и покрасить ячейку
    в красный цвет.
     */
    #endregion

    /// <summary>
    /// ViewModel для сбора информации о координатах потребителей и рассчёта трансформаторных подстанций
    /// </summary>
    public class SubstationsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;


        public District SelectedDistrict { get; set; } = new District();
        
        public Substation SelectedSubstation { get; set; } = new Substation();


        public SubstationsViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;

            _messageBus.Receive<DistrictMessage>(this, async message =>
            {
                SelectedDistrict = new District();
                SelectedDistrict.Substations = new List<Substation>();
                SelectedDistrict = message.SharedDistrict;
            });
        }

        public ICommand DetermineNumberOfSubstations => new DelegateCommand(() =>
        {
            SelectedDistrict.Substations = SelectedDistrict.DetermineSubstationsList();
            if (SelectedDistrict.Substations==default)
            {
                MessageBox.Show("Значення у полях мають бути більше нуля");
            }
        });

        public ICommand GoForLengths => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new SubstationLengthInfoPage());
            await _messageBus.SendTo<LengthHandlingViewModel>(new SubstationMessage(SelectedSubstation));
        });

        public ICommand Optimize => new AsyncCommand(async () =>
        {
            foreach (var ts in SelectedDistrict.Substations)
            {
                List<List<int>> hyy = ts.DefineCombinationsPerSubstation(SelectedDistrict.NumberOfTransformers, SelectedDistrict.TransformerLoad, SelectedDistrict.MinCoeffOfLoadSubstation, SelectedDistrict.MaxCoeffOfLoadSubstation, SelectedDistrict.MaxCalbeLength);
            }
        });
    }
}
