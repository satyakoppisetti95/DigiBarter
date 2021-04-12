using DigiBarter.ViewModels;
using DigiBarter.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace DigiBarter
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(TradePage), typeof(TradePage));
            Routing.RegisterRoute(nameof(AddItemPage), typeof(AddItemPage));
            Routing.RegisterRoute(nameof(RequestApprovalPage), typeof(RequestApprovalPage));
            Routing.RegisterRoute("editprofilepage", typeof(EditProfilePage));
        }

    }
}
