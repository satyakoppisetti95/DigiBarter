using DigiBarter.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigiBarter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            Title = "DigiBarter";
            InitializeComponent();
        }

        private async void RegisterBtn_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//register");
        }

        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {
            
            string email = emailInput.Text;
            string password = PasswordInput.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Invalid credentials", "Dismiss");
                return;
            }

            var fireLogin = DependencyService.Get<IAuth>();
            string token = await fireLogin.LoginWithEmailPassword(email, password);
            emailInput.Text = "";
            PasswordInput.Text = "";
            if (token.Length > 300)
            {
                await Shell.Current.GoToAsync("//main");
            }
            else
            {
                await DisplayAlert("Error", "Something went wrong please try again", "Cancel");
            }
        }
    }
}