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
    [QueryProperty(nameof(RequestId), nameof(RequestId))]
    public partial class RequestApprovalPage : ContentPage
    {
        public RequestApprovalPage()
        {
            InitializeComponent();
        }

        private string requestId;
        private TradeRequestItem request;

        public string RequestId
        {
            get
            {
                return requestId;
            }
            set
            {
                requestId = value;
                LoadRequestId(value);
            }
        }

        async void LoadRequestId(string requestId)
        {
            try { 
            var document = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("requests")
                                         .Document(requestId)
                                         .GetAsync();
            request = document.ToObject<TradeRequestItem>();

            if (request == null)
            {
                await DisplayAlert("Oh! Oh!", "Invalid Request", "OK");
                return;
            }
            

            ProfilePicHolder.Source = request.user_photo_url;
            userNameText.Text = request.user_name;
            requestedTimeText.Text = request.request_time;
            itemText.Text = request.item_name;
            
            }catch(Exception e)
            {

            }

        }

        private async void ApproveBtn_Clicked(object sender, EventArgs e)
        {
            await CrossCloudFirestore.Current
                                        .Instance
                                        .Collection("requests")
                                        .Document(requestId).UpdateAsync(new {is_approved=true,is_hidden=true});
            await DisplayAlert("Success", "Trade Request approved. User can now see your contact details", "OK");
            await Shell.Current.GoToAsync("..");
        }

        private async void DeclineBtn_Clicked(object sender, EventArgs e)
        {
            await CrossCloudFirestore.Current
                                        .Instance
                                        .Collection("requests")
                                        .Document(requestId).UpdateAsync(new { is_declined = true,is_hidden=true });
            await DisplayAlert("Success", "Trade Request Declined", "OK");
            await Shell.Current.GoToAsync("..");
        }
    }
}