using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using WpfPaging.Pages;

namespace WpfPaging.Services
{
    public class PageService
    {

        public event Action<Page> OnPageChanged;
        private Page _lastPage;
      

        public Stack<Page> _history;
        public bool CanGoBack => _history.Skip(1).Any();
        public PageService()
        {
            _history = new Stack<Page>();
        }

        public void ChangePage(Page page)
        {
            OnPageChanged?.Invoke(page);
            _history.Push(page);
        }

        public void GoBack()
        {
            _history.Pop();
            var page = _history.Peek();
            OnPageChanged?.Invoke(page);
        }


    }
}
