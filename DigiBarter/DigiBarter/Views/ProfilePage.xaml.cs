using DigiBarter.Interfaces;
using DigiBarter.Models;
using Plugin.CloudFirestore;
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
    public partial class ProfilePage : ContentPage
    {
        private Authenticateduser user;
        public ProfilePage()
        {
            Title = "Profile";
            InitializeComponent();
            //LoadUser();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadUser();
        }

        public async void LoadUser()
        {
            //this.user = await DependencyService.Get<IFireDb>().getUserProfile();
            try { 
                var document = await CrossCloudFirestore.Current
                                             .Instance
                                             .Collection("users")
                                             .Document(DependencyService.Get<IAuth>().getCurrentUserUID())
                                             .GetAsync();
                user = document.ToObject<Authenticateduser>();
                if (user == null) { return; }
                if (string.IsNullOrWhiteSpace(user.first_name))
                {
                    WelcomeLabel.Text = "Welcome";
                }
                else
                {
                    WelcomeLabel.Text = "Welcome " + user.first_name;
                }

                if (!string.IsNullOrWhiteSpace(user.photo_url))
                {
                    profilePicHolder.Source = user.photo_url;
                }
            }catch(Exception e)
            {

            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IAuth>().SignOut();
            await Shell.Current.GoToAsync("//login");

        }


        private async void EditProfileBtn_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("editprofilepage");
        }
    }
}