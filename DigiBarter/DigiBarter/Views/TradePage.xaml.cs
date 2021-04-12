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
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public partial class TradePage : ContentPage
    {
        public TradePage()
        {
            Title = "Trade";
            InitializeComponent();
            
        }

        private Authenticateduser user = null;
        private string itemId;
        private TradeItem item;

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadUser()
        {
            //this.user = await DependencyService.Get<IFireDb>().getUserProfile();
            try
            {
                var document = await CrossCloudFirestore.Current
                                             .Instance
                                             .Collection("users")
                                             .Document(DependencyService.Get<IAuth>().getCurrentUserUID())
                                             .GetAsync();
                user = document.ToObject<Authenticateduser>();
               
            }
            catch (Exception e)
            {

            }
        }

        private async void LoadItemId(string item_id)
        {
            var document = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("items")
                                         .Document(item_id)
                                         .GetAsync();
            item = document.ToObject<TradeItem>();

            if (item == null)
            {
                await DisplayAlert("Oh! Oh!", "Invalid Item", "OK");
                return;
            }

            TitleText.Text = item.title;
            ItemPicHolder.Source = item.photo_url;
            ProfilePicHolder.Source = item.user_photo_url;
            userNameText.Text = item.user_name;
            postedTimeText.Text = item.posted_time;
            DescText.Text = item.description;

            if (DependencyService.Get<IAuth>().getCurrentUserUID().Equals(item.user_id))
            {
                TradeButton.IsVisible = false;
            }

            LoadUser();
            HandleTradeStatus();
        }

        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private async void HandleTradeStatus()
        {
            try
            {
                var query = await CrossCloudFirestore.Current
                            .Instance
                            .Collection("requests").WhereEqualsTo("owner_id", item.user_id)
                            .WhereEqualsTo("user_id", DependencyService.Get<IAuth>().getCurrentUserUID())
                            .GetAsync();

                if (query.ToObjects<TradeRequestItem>().Count() > 0)
                {
                    TradeRequestItem req_item = query.ToObjects<TradeRequestItem>().First();

                    tradeStatusText.IsVisible = true;
                    TradeButton.IsVisible = false;
                    if (req_item.is_declined)
                    {
                        tradeStatusText.Text = "Trade Request Denied by user";
                    }
                    if (req_item.is_approved)
                    {
                        contactText.Text = user.contact;
                        addressText.Text = user.address;
                        tradeStatusText.Text = "Trade Request Approved";
                    }
                }
            }catch(Exception e)
            {

            }
        }

        private async void TradeButton_Clicked(object sender, EventArgs e)
        {
            if (user == null)
            {
                await DisplayAlert("Oh! Oh!", "Please complete your profile to trade", "OK");
                return;
            }

            string request_id = RandomString(20);

            await CrossCloudFirestore.Current
                        .Instance
                        .Collection("requests")
                        .Document(request_id)
                        .SetAsync(new
                        {
                            request_id = request_id,
                            item_id = item.item_id,
                            item_name = item.title,
                            user_id = DependencyService.Get<IAuth>().getCurrentUserUID(),
                            owner_id = item.user_id,
                            user_name = user.first_name+ " "+user.last_name,
                            user_photo_url = user.photo_url,
                            is_declined=false,
                            is_approved=false,
                            is_hidden=false,
                            request_time = DateTime.Now.ToString("dddd, dd MMMM yyyy"),
                            sortable_time = DateTime.Now.Ticks
                        });
            TradeButton.IsVisible = false;
            tradeStatusText.IsVisible = true;
        }
    }
}