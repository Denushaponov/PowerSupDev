using DevExpress.Mvvm;
using System.Windows.Controls;
using System.Windows.Input;
using WpfPaging.Pages;
using WpfPaging.Services;

namespace WpfPaging.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly PageService _pageService;

        public Page PageSource { get; set; }

        public MainViewModel(PageService pageService)
        {
            _pageService = pageService;
            _pageService.OnPageChanged += (page) => PageSource = page;
            _pageService.ChangePage(new MainMenu());
        }

        public ICommand GoBack => new DelegateCommand(() =>
        {
            _pageService.GoBack();
        }, () => _pageService.CanGoBack);

    }
}
