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
    public partial class Register : ContentPage
    {
        public Register()
        {
            Title = "DigiBarter";
            InitializeComponent();
        }

        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//login");
             
        }

        private async void RegisterBtn_Clicked(object sender, EventArgs e)
        {
            
            string email = emailInput.Text;
            string password = PasswordInput.Text;
            string cpassword = ConfirmPasswordInput.Text;


            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(cpassword) )
            {
                await DisplayAlert("Error", "Invalid credentials", "Dismiss");
                return;
            }

            if (!password.Equals(cpassword))
            {
                await DisplayAlert("Error", "Passwords do not match", "Dismiss");
                return;
            }


            var fireLogin = DependencyService.Get<IAuth>();
            string token = await fireLogin.SignupWithEmailPassword(email, password);
            emailInput.Text = "";
            PasswordInput.Text = "";
            ConfirmPasswordInput.Text = "";
            if (token.Length > 200)
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