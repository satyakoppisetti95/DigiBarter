using DigiBarter.Interfaces;
using DigiBarter.Models;
using DigiBarter.Services;
using Plugin.CloudFirestore;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigiBarter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddItemPage : ContentPage
    {
        private Authenticateduser user;

        FirebaseStorageHelper firebaseStorageHelper = new FirebaseStorageHelper();
        MediaFile file = null;
        string remote_pic_url = "";

        public AddItemPage()
        {
            Title = "Add Item";
            InitializeComponent();
            LoadUser();
        }

        public async void LoadUser()
        {
            //this.user = await DependencyService.Get<IFireDb>().getUserProfile();
            var document = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("users")
                                         .Document(DependencyService.Get<IAuth>().getCurrentUserUID())
                                         .GetAsync();
            user = document.ToObject<Authenticateduser>();

            if (user == null)
            {
                showAddProfileDetailsMsg();
            }

        }

        private async void showAddProfileDetailsMsg()
        {
            await DisplayAlert("Oh! Oh!", "Please complete your profile details before adding an Item", "OK");
        }

        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async void galleryPickBtn_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });
                if (file == null)
                    return;
                ItemPicHolder.Source = ImageSource.FromStream(() =>
                {
                    var imageStram = file.GetStream();
                    return imageStram;
                });
            }
            catch (Exception ex)
            {

            }
        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {
            string title = titleInput.Text;
            string desc = descInput.Text;
            string type = "TRADE";
            if (picker.SelectedIndex == 0)
            {
                type = "FREE";
            }

            if (string.IsNullOrWhiteSpace(title) ||
               string.IsNullOrWhiteSpace(desc))
            {
                await DisplayAlert("Oops", "Please enter all values", "OK");
                return;
            }
            if (file == null)
            {
                await DisplayAlert("Oops", "Please select an image for the item", "OK");
                return;
            }

            if(string.IsNullOrWhiteSpace(user.first_name) || 
                string.IsNullOrWhiteSpace(user.last_name) ||
                string.IsNullOrWhiteSpace(user.photo_url))
            {
                showAddProfileDetailsMsg();
                return;
            }
           
            remote_pic_url = await firebaseStorageHelper.UploadFile(file.GetStream(), DependencyService.Get<IAuth>().getCurrentUserUID() + "_" + Path.GetFileName(file.Path), "itempics");
            

            string item_id = RandomString(20);

            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("items")
                         .Document(item_id)
                         .SetAsync(new
                         {
                             item_id = item_id,
                             user_id = DependencyService.Get<IAuth>().getCurrentUserUID(),
                             title = title,
                             description = desc,
                             type = type,
                             photo_url = remote_pic_url,
                             user_name = user.first_name + " " + user.last_name,
                             user_photo_url = user.photo_url,
                             posted_time = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                             sortable_time = DateTime.Now.Ticks
                         });

            await Shell.Current.GoToAsync("..");
        }
    }
}