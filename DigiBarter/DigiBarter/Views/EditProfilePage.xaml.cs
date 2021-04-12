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
using Plugin.Media;
using Plugin.Media.Abstractions;
using DigiBarter.Services;
using System.IO;

namespace DigiBarter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditProfilePage : ContentPage
    {
        private Authenticateduser user;

        FirebaseStorageHelper firebaseStorageHelper = new FirebaseStorageHelper();
        MediaFile file = null;
        string remote_pic_url = "";

        public EditProfilePage()
        {
            Title = "Edit Profile";
            InitializeComponent();
            LoadUser();
        }

        public async void LoadUser()
        {
            try
            {
                //this.user = await DependencyService.Get<IFireDb>().getUserProfile();
                var document = await CrossCloudFirestore.Current
                                             .Instance
                                             .Collection("users")
                                             .Document(DependencyService.Get<IAuth>().getCurrentUserUID())
                                             .GetAsync();
                user = document.ToObject<Authenticateduser>();

                if (!string.IsNullOrWhiteSpace(user.first_name))
                {
                    firstNameInput.Text = user.first_name;
                }
                if (!string.IsNullOrWhiteSpace(user.last_name))
                {
                    lasttNameInput.Text = user.last_name;
                }
                if (!string.IsNullOrWhiteSpace(user.contact))
                {
                    phoneInput.Text = user.contact;
                }
                if (!string.IsNullOrWhiteSpace(user.address))
                {
                    addressInput.Text = user.address;
                }
                if (!string.IsNullOrWhiteSpace(user.photo_url))
                {
                    remote_pic_url = user.photo_url;
                    profilePicHolder.Source = user.photo_url;
                }
            }catch(Exception e)
            {

            }

        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {
            string first_name = firstNameInput.Text;
            string last_name = lasttNameInput.Text;
            string phone = phoneInput.Text;
            string address = addressInput.Text;

            if(string.IsNullOrWhiteSpace(first_name) || 
                string.IsNullOrWhiteSpace(last_name) || 
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(address))
            {
                await DisplayAlert("Oops", "Please enter all values", "Dismiss");
                return;
            }

           

            if (file != null)
            {
                remote_pic_url =  await firebaseStorageHelper.UploadFile(file.GetStream(), DependencyService.Get<IAuth>().getCurrentUserUID()+"_" + Path.GetFileName(file.Path), "profilepics");
            }

            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("users")
                         .Document(DependencyService.Get<IAuth>().getCurrentUserUID())
                         .SetAsync(new { first_name = first_name,last_name=last_name, address=address,contact=phone, photo_url=remote_pic_url });

            await Shell.Current.GoToAsync("..");
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
                profilePicHolder.Source = ImageSource.FromStream(() =>
                {
                    var imageStram = file.GetStream();
                    return imageStram;
                });
            }
            catch (Exception ex)
            {
                
            }
        }

        private void cameraPickBtn_Clicked(object sender, EventArgs e)
        {

        }
    }
}