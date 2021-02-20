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

        /// <summary>
        /// Выбранный микрорайон (в DistrictMenuVM), присылается для изменений
        /// </summary>
        public District SelectedDistrict { get; set; } = new District();

        public ApartmentBuilding SelectedApartmentBuilding { get; set; }

        /// <summary>
        /// Комманда добавления нового жилого дома
        /// </summary>
        public ICommand AddCommand => new AsyncCommand(async () =>
        {
            ApartmentBuilding apartmentBuilding = new ApartmentBuilding();
            SelectedDistrict.Building.ApartmentBuildings.Insert(0, apartmentBuilding);
            SelectedApartmentBuilding = apartmentBuilding;
        
        }
        );

      
 

        /// <summary>
        /// Комманда удаления выбранного жилого дома
        /// </summary>
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

            // Получение данных о микрорайоне
            _messageBus.Receive<DistrictMessage>(this, async message =>
            {
                // присваивание присланного из DistrictMenuVM микрорайона в качестве выбранного
                SelectedDistrict = message.SharedDistrict;
            });

           
        }

        /// <summary>
        /// Отправка микрорайона с отредактированной коллекцией ApartmentBuildings
        /// </summary>
        public ICommand SendApartmentBuildings => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<DistrictMenuViewModel>(new DistrictMessage(SelectedDistrict));
            await _eventBus.Publish(new SaveEvent());
        });

    }
}
