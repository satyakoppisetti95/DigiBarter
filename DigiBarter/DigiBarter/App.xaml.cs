using DigiBarter.Interfaces;
using DigiBarter.Services;
using DigiBarter.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigiBarter
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();

            MainPage = new AppShell();

            if (DependencyService.Get<IAuth>().IsUserLoggedIn())
            {
                Shell.Current.GoToAsync("//main");
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
