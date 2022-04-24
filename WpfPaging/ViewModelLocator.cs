
using DistrictSupplySolution.ViewModels;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using WpfPaging.Services;
using WpfPaging.ViewModels;

namespace WpfPaging
{
    public class ViewModelLocator
    {
        private static ServiceProvider _provider;

        public static void Init()
        {
            var services = new ServiceCollection();

            services.AddTransient<MainViewModel>();
            services.AddTransient<CommercialsViewModel>();
            services.AddTransient<ApartmentsViewModel>();
            services.AddTransient<MainMenuViewModel>();
            services.AddTransient<LoadOfDistrictViewModel>();
            services.AddTransient<AbstractBuildingViewModel>();
            services.AddTransient<SubstationsViewModel>();
            services.AddScoped<DistrictMenuViewModel>();

            services.AddTransient<Repository>();
           services.AddSingleton(new LiteDatabase("Data/empty.db"));

            services.AddSingleton<PageService>();
            services.AddSingleton<EventBus>();
            services.AddSingleton<MessageBus>();



            _provider = services.BuildServiceProvider();

            foreach (var item in services)
            {
                _provider.GetRequiredService(item.ServiceType);
            }
        }

        public MainViewModel MainViewModel => _provider.GetRequiredService<MainViewModel>();
        public MainMenuViewModel MainMenuViewModel => _provider.GetRequiredService<MainMenuViewModel>();
        public CommercialsViewModel CommercialsViewModel => _provider.GetRequiredService<CommercialsViewModel>();
        public ApartmentsViewModel ApartmentsViewModel => _provider.GetRequiredService<ApartmentsViewModel>();
        public DistrictMenuViewModel DistrictViewModel => _provider.GetRequiredService<DistrictMenuViewModel>();
        public LoadOfDistrictViewModel LoadOfDistrictViewModel => _provider.GetRequiredService<LoadOfDistrictViewModel>();
        public AbstractBuildingViewModel AbstractBuildingViewModel => _provider.GetRequiredService<AbstractBuildingViewModel>();
        public SubstationsViewModel SubstationsViewModel => _provider.GetRequiredService<SubstationsViewModel>();


    }
}
