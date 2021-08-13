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
    public class AbstractBuildingViewModel:BindableBase
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;

        /// <summary>
        /// Здесь хранится копия здания если нужно будет откатить значения назад к первоначальным
        /// </summary>
        private AbstractBuilding _backupSelectedAbstractBuilding;

        public AbstractBuilding SelectedAbstractBuilding { get; set; }
        public SpecialCoefficientOfMax SelectedSpecialCoefficientOfMax { get; set; }

        public AbstractBuildingViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;
            
            // Получение данных о  здании в для которого пользователь будет ввобдить свои коєфициенті участия в максимуме
            _messageBus.Receive<AbstractBuildingMessage>(this, async message =>
            {
                // присваивание присланного из DistrictLoadVM асбтрактного здания для отображения и редактирования его данных
                SelectedAbstractBuilding = message.SharedAbstractBuilding;
                _backupSelectedAbstractBuilding = message.SharedAbstractBuilding;
            });


        }

        
        public ICommand SaveCommand => new AsyncCommand(async () =>
        {
            byte i=0;
            foreach (var cm in SelectedAbstractBuilding.SpecialConsumerCoefficientsOfMax)
            {
                // добавляю особекнную отметку к тем потребителям для которых пользователь 
                if (cm.CoefficientOfMax == 0)
                    i++;
            }
            if (i == 0)
            { SelectedAbstractBuilding.SideNote = "Особливий визначений"; }
            await _messageBus.SendTo<LoadOfDistrictViewModel>(new AbstractBuildingMessage(SelectedAbstractBuilding));
            
        });

        public ICommand CancelChanges => new AsyncCommand(async () =>
        {
            SelectedAbstractBuilding = null;
        });

       


    }

}

    
