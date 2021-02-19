using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfPaging.DistrictObjects;
using WpfPaging.Events;
using WpfPaging.Messages;
using WpfPaging.Pages;
using WpfPaging.Services;

namespace WpfPaging.ViewModels
{
    public class DistrictMenuViewModel:BindableBase
    {

        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;
        private readonly Repository _repository;



        public ObservableCollection<District> Districts { get; set; } = new ObservableCollection<District>();
           public District SelectedDistrict { get; set; } = new District();

        public DistrictMenuViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus, Repository repository)
        {

            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;
            _repository = repository;

            _eventBus.Subscribe<OnSave<District>>(OnSaveDistrict);
            _eventBus.Subscribe<OnDelete<District>>(OnDeleteDistrict);

            Districts = new ObservableCollection<District>();
            repository.FindAll<District>().ContinueWith(s =>
              {
                  Districts = new ObservableCollection<District>(s.Result);
              }, TaskContinuationOptions.OnlyOnRanToCompletion);
           
            Districts.Add( new District { Title = "Введіть назву" });
            SelectedDistrict = Districts[0];

            _messageBus.Receive<DistrictMessage>(this, async message =>
            {
                SelectedDistrict = message.SharedDistrict;
            });

        }

        private Task OnDeleteDistrict(OnDelete<District> arg)
        {
            var item = Districts.FirstOrDefault(s => s.Id == arg.Id);
           if (item!=null)
            {
                Districts.Remove(item);
            }
            return Task.CompletedTask;
        }

        private  Task OnSaveDistrict(OnSave<District> arg)
        {
            foreach (var d in Districts)
            {
            if (arg.Id==d.Id)
             {
                    Districts.Remove(d);
                    Districts.Add(arg.Entity);
                    
               }
            }
           
         
            return Task.CompletedTask;
        }

        public  ICommand RemoveDistrict => new AsyncCommand<District>(async(district) =>
        {
           await _repository.Remove<District>(district.Id);
            Districts.Remove(district);
        }, (district) => district != null);

        public ICommand AddDistrict => new DelegateCommand(() =>
        {
            Districts.Add(new District { Title = "Введіть назву" });
        });


        // Команда сохраняет микрорайон по нажатию на кнопку
        public ICommand SaveDistrict => new AsyncCommand(async() =>
        {
            if (SelectedDistrict==null)
            {
                MessageBox.Show("Оберіть мікрорайон для збереження");
                return;
            }
            MessageBox.Show("Мікрорайон " + SelectedDistrict.Title + " збережено");
            await _repository.Save(SelectedDistrict, SelectedDistrict.Id);
        });


        public ICommand GoForApartmentBuildings => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new Apartments());
            await _messageBus.SendTo<ApartmentsViewModel>(new DistrictMessage(SelectedDistrict));
            await _eventBus.Publish(new LeaveFromFirstPageEvent());
        });

      



    }
}
