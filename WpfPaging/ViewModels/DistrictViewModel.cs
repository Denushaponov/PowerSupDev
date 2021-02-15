using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using WpfPaging.DistrictObjects;
using WpfPaging.Services;

namespace WpfPaging.ViewModels
{
    public class DistrictViewModel:BindableBase
    {

        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;




        public ObservableCollection<District> Districts { get; set; } = new ObservableCollection<District>();

        public DistrictViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;

            Districts.Add(new District
            { Title = "Holland" });
        }

        public ICommand OpenDistrict => new DelegateCommand(() =>
        {
            //
        }
        );

        public ICommand RemoveDistrict => new DelegateCommand<District>((district) =>
        {
            Districts.Remove(district);
        }, (district) => district != null);

        public ICommand AddDistrict => new DelegateCommand(() =>
        {

        });


    }
}
