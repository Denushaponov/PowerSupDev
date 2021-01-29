using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WpfPaging.Events;
using WpfPaging.Messages;
using WpfPaging.Pages;
using WpfPaging.Services;

namespace WpfPaging.ViewModels
{
   public class ApartmentsViewModel:BindableBase
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;

        public string LogText { get; set; }

        public ApartmentsViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;
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
