using DevExpress.Mvvm;
using DistrictSupplySolution.DistrictObjects;
using DistrictSupplySolution.Messages;
using DistrictSupplySolution.MessageWindows;
using DistrictSupplySolution.Pages;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfPaging.Events;
using WpfPaging.Messages;
using WpfPaging.Pages;
using WpfPaging.Services;

namespace DistrictSupplySolution.ViewModels
{
    public class LengthHandlingViewModel: BindableBase
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;

        /// <summary>
        /// Здесь хранится копия здания если нужно будет откатить значения назад к первоначальным
        /// </summary>
        private Substation _backupSelectedSubstation;
        public Substation SelectedSubstation { get; set; }
        public OptimizationDataBuilding SelectedOptiBuilding { get; set; }
        public LengthHandlingViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;

            _messageBus.Receive<SubstationMessage>(this, async message =>
            {
                  SelectedSubstation = message.SharedSubstation;
                _backupSelectedSubstation = message.SharedSubstation;
            });
        }


        public ICommand SaveCommand => new AsyncCommand(async () =>
        {
            byte i = 0;
            foreach (var cm in SelectedSubstation.OptimizationDataBuildings)
            {
                 if (cm.CableLength == 0)
                    i++;
            }
            if (i != 0)
            { SelectedSubstation.IsLengthsCompleted = false; }
            else
            { SelectedSubstation.IsLengthsCompleted = true; }
            await _messageBus.SendTo<SubstationsViewModel>(new SubstationMessage(SelectedSubstation));

        });

        public ICommand CancelChanges => new AsyncCommand(async () =>
        {
            SelectedSubstation = _backupSelectedSubstation;
        });
    }
}
