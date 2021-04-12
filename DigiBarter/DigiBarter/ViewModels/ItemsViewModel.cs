using DigiBarter.Models;
using DigiBarter.Views;
using Plugin.CloudFirestore;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DigiBarter.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private TradeItem _selectedItem;

        public ObservableCollection<TradeItem> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<TradeItem> ItemTapped { get; }

        public ItemsViewModel()
        {
            Title = "Explore";
            Items = new ObservableCollection<TradeItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<TradeItem>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            

            try
            {
                var query = await CrossCloudFirestore.Current
                                         .Instance
                                         .Collection("items")
                                         .OrderBy("sortable_time", true)
                                         .GetAsync();

                var remote_items = query.ToObjects<TradeItem>();
                Items.Clear();
                foreach(var r_item in remote_items)
                {
                    Items.Add(r_item);
                }
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public TradeItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(AddItemPage));
        }

        async void OnItemSelected(TradeItem item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(TradePage)}?{nameof(ItemDetailViewModel.ItemId)}={item.item_id}");
        }
    }
}