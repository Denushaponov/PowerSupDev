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

    /// <summary>
    /// ViewModel для сбора информации о координатах потребителей и рассчёта трансформаторных подстанций
    /// </summary>
    public class SubstationsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;

        public District SelectedDistrict { get; set; }
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
            SelectedDistrict.Substations = SelectedDistrict.DetermineSubstationsList(0.7);
        });
    }
}
