using DevExpress.Mvvm;
using System;
using System.Windows.Input;
using WpfPaging.Events;
using WpfPaging.Messages;
using WpfPaging.Pages;
using WpfPaging.Services;
using WpfPaging.DistrictObjects;
using System.Collections.ObjectModel;

namespace WpfPaging.ViewModels
{
    public class CommercialsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;

     
        /// <summary>
        /// Микрорайон который присылается, редактируется и отсылается
        /// </summary>
        public District SelectedDistrict { get; set; } = new District();

        /// <summary>
        /// Выбранное коммерческое здание, которое редактируется
        /// </summary>
        public CommercialBuilding SelectedCommercialBuilding { get; set; }

        public CommercialsViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;

            SelectedDistrict.Building.CommercialBuildings= new ObservableCollection<CommercialBuilding> { };

            _messageBus.Receive<DistrictMessage>(this, async message =>
            {
                SelectedDistrict = message.SharedDistrict;
            });



        }

        /// <summary>
        /// Добавление нового коммерческого здания
        /// </summary>
        public ICommand AddCommand => new AsyncCommand(async () =>
        {
            CommercialBuilding commercialBuilding = new CommercialBuilding();
            SelectedDistrict.Building.CommercialBuildings.Insert(0, commercialBuilding);
            SelectedCommercialBuilding = commercialBuilding;
        }
     );

        /// <summary>
        /// Удаление выбраноого коммерческого здания
        /// </summary>
        public ICommand Remove
        {
            get
            {
                return new DelegateCommand<CommercialBuilding>((commercialBuilding) =>
                {
                    SelectedDistrict.Building.CommercialBuildings.Remove(commercialBuilding);

                }, (commercialBuilding) => commercialBuilding != null);
            }
        }

        /// <summary>
        /// Отправка данных в главное меню для сохранения и сохранение
        /// </summary>
        public ICommand SendCommercialBuildings => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<DistrictMenuViewModel>(new DistrictMessage(SelectedDistrict));
            await _eventBus.Publish(new SaveEvent());
        });

        public ICommand ExecuteCalculation => new AsyncCommand(async () =>
        {
            SelectedDistrict.CalculateCommercialBuildings();
        }
       );


    }
}
