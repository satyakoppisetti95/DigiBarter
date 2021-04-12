using DigiBarter.Models;
using DigiBarter.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DigiBarter.Views
{
    public partial class NewItemPage : ContentPage
    {
        public TradeItem Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}