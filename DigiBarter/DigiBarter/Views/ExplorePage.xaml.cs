using DigiBarter.Models;
using Plugin.CloudFirestore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigiBarter.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExplorePage : ContentPage
    {
        private ObservableCollection<TradeItem> items { get; set; }
        public ExplorePage()
        {
            InitializeComponent();
            this.BindingContext = this;

            items = new ObservableCollection<TradeItem>();

            /*
            TradeItem t = new TradeItem();
            t.title = "Item1";
            t.description = "Description";
            t.photo_url = "https://images.pexels.com/photos/3298037/pexels-photo-3298037.jpeg?auto=compress&cs=tinysrgb&dpr=2&w=500";
            t.type = "FREE";
            items.Add(t);
            ItemsList.ItemsSource = items;
            */

        }

        public async void LoadItems()
        {
            //this.user = await DependencyService.Get<IFireDb>().getUserProfile();
            var query = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("items")
                                         .OrderBy("Value", false)
                                         .GetAsync();

            var remote_items = query.ToObjects<TradeItem>();
            foreach(var r_item in remote_items)
            {
                items.Add(r_item);
            }
        }
    }
}