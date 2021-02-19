using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using WpfPaging.Events;
using WpfPaging.Messages;
using WpfPaging.Pages;
using WpfPaging.Services;
using WpfPaging.DistrictObjects;
using GalaSoft.MvvmLight.Command;
using System.Linq;
using System.Threading.Tasks;

namespace WpfPaging.ViewModels
{
    public class ApartmentsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;

        public District SelectedDistrict { get; set; } = new District();
        public ApartmentBuilding SelectedApartmentBuilding { get; set; }

        public ObservableCollection<LoadsApartmentBuilding> LoadsOfApartmentBuildings { get; set; }


        // Создаю комманду которая очистит параметры лифтов
        
        
       



        public ICommand AddCommand => new AsyncCommand(async () =>
        {
            ApartmentBuilding apartmentBuilding = new ApartmentBuilding();
            SelectedDistrict.Building.ApartmentBuildings.Insert(0, apartmentBuilding);
            SelectedApartmentBuilding = apartmentBuilding;
        }
        );

        public ICommand Remove
        {
            get 
            {
                return new DelegateCommand<ApartmentBuilding>((apartmentBuilding) =>
                {
                    SelectedDistrict.Building.ApartmentBuildings.Remove(apartmentBuilding);

                }, (apartmentBuilding) => apartmentBuilding != null);
                }
        }

       


        /// <summary>
        /// Команды и параметры для взаимодействия между окнами
        /// </summary>
        /// <param name="pageService"></param>
        /// <param name="eventBus"></param>
        /// <param name="messageBus"></param>

        public ApartmentsViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;

            SelectedDistrict.Building.ApartmentBuildings = new ObservableCollection<ApartmentBuilding> { };

            _messageBus.Receive<DistrictMessage>(this, async message =>
            {
                SelectedDistrict = message.SharedDistrict;
            });

           
        }

        public ICommand SendApartmentBuildings => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<DistrictMenuViewModel>(new DistrictMessage(SelectedDistrict));
           

        });


        // Команда выхода со страницы в главное меню
        public ICommand ChangePage2 => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new MainMenu());

            await _eventBus.Publish(new LeaveFromFirstPageEvent());
        });

        // Комманда перехода на страницу Жилые здания
        public ICommand ChangePage => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new Commercials());

            await _eventBus.Publish(new LeaveFromFirstPageEvent());
        });

        

    }
}
