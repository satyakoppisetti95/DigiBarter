using DigiBarter.Interfaces;
using DigiBarter.Models;
using Plugin.CloudFirestore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigiBarter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RequestsPage : ContentPage
    {
        public ObservableCollection<TradeRequestItem> Items { get; set; }

        public RequestsPage()
        {
            Title = "Requests";
            InitializeComponent();

            Items = new ObservableCollection<TradeRequestItem>();
           
            MyListView.ItemsSource = Items;
            LoadRequests();
        }

        async void LoadRequests()
        {
            try
            {
                var query = await CrossCloudFirestore.Current
                                     .Instance
                                     .Collection("requests")
                                     .WhereEqualsTo("owner_id", DependencyService.Get<IAuth>().getCurrentUserUID())
                                     .WhereEqualsTo("is_hidden",false)
                                     .GetAsync();
                var remote_items = query.ToObjects<TradeRequestItem>();
                Items.Clear();
                foreach (var r_item in remote_items)
                {
                    Items.Add(r_item);
                }
            }catch(Exception e)
            {

            }
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadRequests();
        }


        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            //await DisplayAlert("Item Tapped", "An item was tapped."+((TradeRequestItem)e.Item).user_name, "OK");
            string request_id = ((TradeRequestItem)e.Item).request_id;
            await Shell.Current.GoToAsync($"{nameof(RequestApprovalPage)}?{nameof(RequestApprovalPage.RequestId)}={request_id}");



            //Deselect Item
           // ((ListView)sender).SelectedItem = null;
        }

        
    }
}
