using DevExpress.Mvvm;
using System;
using System.Windows.Input;
using WpfPaging.Events;
using WpfPaging.Messages;
using WpfPaging.Pages;
using WpfPaging.Services;

namespace WpfPaging.ViewModels
{
    public class CommercialsViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;

        public string LogText { get; set; }

        public CommercialsViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;
        }

        public ICommand ChangePage => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new MainMenu());

            await _eventBus.Publish(new LeaveFromFirstPageEvent());
        });

        public ICommand ToApartmentsPage => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new Apartments());

            await _eventBus.Publish(new LeaveFromFirstPageEvent());
        });

        public ICommand SendLog => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<MainMenuViewModel>(new TextMessage(LogText));
            //await _messageBus.SendTo<object>(new TextMessage(LogText));

        });
    }
}
