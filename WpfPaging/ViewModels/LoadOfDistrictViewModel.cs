using DevExpress.Mvvm;
using DistrictSupplySolution.DistrictObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
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
            Console.WriteLine("");
        }

        // Начало рассчёта
        public ICommand SendAbstractBuildings => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<DistrictMenuViewModel>(new DistrictMessage(SelectedDistrict));
            await _eventBus.Publish(new SaveEvent());
        });

        public ICommand CalculateDistrict => new DelegateCommand(() =>
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





    }
}
