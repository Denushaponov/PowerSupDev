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

        // Тестовый параметр , пример того как пересылать данные между вьюмоделями
        public string LogText { get; set; }
        public ObservableCollection<CommercialBuilding> CommercialBuildings { get; set; }
        public CommercialBuilding SelectedCommercialBuilding { get; set; }

        public CommercialsViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;

            CommercialBuildings = new ObservableCollection<CommercialBuilding> { };
        }

        public ICommand AddCommand => new AsyncCommand(async () =>
        {
            CommercialBuilding commercialBuilding = new CommercialBuilding();
            CommercialBuildings.Insert(0, commercialBuilding);
            SelectedCommercialBuilding = commercialBuilding;
        }
     );

        public ICommand Remove
        {
            get
            {
                return new DelegateCommand<CommercialBuilding>((commercialBuilding) =>
                {
                   CommercialBuildings.Remove(commercialBuilding);

                }, (commercialBuilding) => commercialBuilding != null);
            }
        }

        public ICommand ClearElectrification => new DelegateCommand(() =>
        {
            SelectedCommercialBuilding.ElectrificationLevel = 0;
        });












        // Команда для выхода в главное меню
        public ICommand ChangePage => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new MainMenu());

            await _eventBus.Publish(new LeaveFromFirstPageEvent());
        });

        // Команда для перехода на страницу жилые здания
        public ICommand ToApartmentsPage => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new Apartments());

            await _eventBus.Publish(new LeaveFromFirstPageEvent());
        });

        // Тестовая комманда для отправки данных в другую вьюмодель, которая реализует команду на приём этих данных
        public ICommand SendLog => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<MainMenuViewModel>(new TextMessage(LogText));
            //await _messageBus.SendTo<object>(new TextMessage(LogText));

        });
    }
}
