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

namespace WpfPaging.ViewModels
{
    public class ApartmentsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;

        public string LogText { get; set; }
        public ObservableCollection<ApartmentBuilding> ApartmentBuildings { get; set; }
        public ApartmentBuilding SelectedApartmentBuilding { get; set; }

        public ObservableCollection<LoadsApartmentBuilding> Appart { get; set; }

        
        // Метод который копирует данные из обьекта одного класса в другой
        public void CopyApartmentBuilding(ApartmentBuilding apartmentBuilding)
        {
            // Для каждого жилого дома в массиве создам новый жилой дом и копируем все параметры
                    LoadsApartmentBuilding l = new LoadsApartmentBuilding();
                    l.PlanNumber = apartmentBuilding.PlanNumber;
                    l.Levels = apartmentBuilding.Levels;
                    l.Entrances = apartmentBuilding.Entrances;
                    l.ApartmentsOnSite = apartmentBuilding.ApartmentsOnSite;
                    l.ElectrificationLevel = apartmentBuilding.ElectrificationLevel;
                    l.ReliabilityCathegory = apartmentBuilding.ReliabilityCathegory;
                    l.FirstElevatorPower = apartmentBuilding.FirstElevatorPower;
                    l.SecondElevatorPower = apartmentBuilding.SecondElevatorPower;
                    l.PompPower = apartmentBuilding.PompPower;
                    Appart.Add(l);
        }


        // Команда которая для кадого жилого здания в коллекции жилых зданий вызывает метод копирующий обьект
        // и не даст пользователю создать двойной дубликат исходной коллекции
        public ICommand AddApart => new DelegateCommand(() =>
        {
            Appart.Clear();
            if (ApartmentBuildings.Count > Appart.Count)
            {
                foreach (var o in ApartmentBuildings)
                { CopyApartmentBuilding(o); }
            }
        });

        // Создаю комманду которая очистит параметры лифта 
        public ICommand ClearElevators => new DelegateCommand(() =>
        {
            
            if (SelectedApartmentBuilding.HasElevators == false)
            {
                SelectedApartmentBuilding.FirstElevatorPower = 0;
                SelectedApartmentBuilding.SecondElevatorPower = 0;
            }
            else return;
        });

        // Создаю комманду которая очистит поля насосы
        public ICommand ClearPomps => new DelegateCommand(() =>
        {
            SelectedApartmentBuilding.PompPower = 0;
        });





        public ICommand AddCommand => new AsyncCommand(async () =>
        {
            ApartmentBuilding apartmentBuilding = new ApartmentBuilding();
            ApartmentBuildings.Insert(0, apartmentBuilding);
            SelectedApartmentBuilding = apartmentBuilding;
        }
        );

        public ICommand Remove
        {
            get 
            {
                return new DelegateCommand<ApartmentBuilding>((apartmentBuilding) =>
                {
                    ApartmentBuildings.Remove(apartmentBuilding);

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
            ApartmentBuildings = new ObservableCollection<ApartmentBuilding> { };
            Appart = new ObservableCollection<LoadsApartmentBuilding> { };
        }
       

        // Команда выхода со страницы в главное меню
        public ICommand ChangePage2 => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new MainMenu());

            await _eventBus.Publish(new LeaveFromFirstPageEvent());
        });

        // Комманда перехода на страницу Жилые здания
        public ICommand ChangePage => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new Apartments());

            await _eventBus.Publish(new LeaveFromFirstPageEvent());
        });

        // Комманда пересылает текст сообщения в главное меню.
        public ICommand SendLog => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<MainMenuViewModel>(new TextMessage(LogText));
            //await _messageBus.SendTo<object>(new TextMessage(LogText));

        });


    }
}
