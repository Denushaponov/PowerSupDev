using DevExpress.Mvvm;
using DistrictSupplySolution.DistrictObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using WpfPaging.DistrictObjects;
using WpfPaging.Messages;
using WpfPaging.Services;

namespace DistrictSupplySolution.ViewModels
{
    public class LoadOfDistrictViewModel:BindableBase
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;

        public District SelectedDistrict { get; set; } = new District();
        public Street SelectedStreet { get; set; }

        public LoadOfDistrictViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;

            _messageBus.Receive<DistrictMessage>(this, async message =>
            {
                // присваивание присланного из DistrictMenuVM микрорайона в качестве выбранного
                SelectedDistrict = message.SharedDistrict;
                SelectedDistrict.Streets = new ObservableCollection<Street> { new Street { Category = "A" }, new Street { Category = "B" }, new Street { Category = "C" } };
                

            });

            }

        public ICommand CalculateDistrict => new DelegateCommand(() =>
        {
            SelectedDistrict.CalculateDistrictPower();
        });




    }
}
