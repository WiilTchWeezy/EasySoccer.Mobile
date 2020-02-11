using Prism;
using Prism.Ioc;
using EasySoccer.Mobile.ViewModels;
using EasySoccer.Mobile.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System;
using EasySoccer.Mobile.API.Session;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace EasySoccer.Mobile
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            Plugin.Iconize.Iconize.With(new Plugin.Iconize.Fonts.FontAwesomeRegularModule())
                                  .With(new Plugin.Iconize.Fonts.FontAwesomeSolidModule());
            if (CurrentUser.Instance.IsLoggedIn)
                await NavigationService.NavigateAsync("/NavigationPage/SoccerPitchSearch");
            else
                await NavigationService.NavigateAsync("Login");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<Login, LoginViewModel>();
            containerRegistry.RegisterForNavigation<SoccerPitchSearch, SoccerPitchSearchViewModel>();
            containerRegistry.RegisterForNavigation<SoccerPitchInfo, SoccerPitchInfoViewModel>();
            containerRegistry.RegisterForNavigation<ScheduleAvaliable, ScheduleAvaliableViewModel>();
            containerRegistry.RegisterForNavigation<RegisterUser, RegisterUserViewModel>();
        }
    }
}
