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
  
    /// <summary>
    /// ViewModel для сбора информации о координатах потребителей и рассчёта трансформаторных подстанций
    /// </summary>
    public class TSViewModel
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;

        
        public District SelectedDistrict { get; set; }

        public TSViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;

            _messageBus.Receive<DistrictMessage>(this, async message =>
            {
                SelectedDistrict = message.SharedDistrict;
            });

        }
    }
}
