//---------------------------------------------------------------------------
//
// <copyright file="CardGameProjectListPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>4/11/2017 11:26:20 AM</createdOn>
//
//---------------------------------------------------------------------------

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using AppStudio.DataProviders.Rss;
using DemoApp.Sections;
using DemoApp.ViewModels;
using AppStudio.Uwp;

namespace DemoApp.Pages
{
    public sealed partial class CardGameProjectListPage : Page
    {
	    public ListViewModel ViewModel { get; set; }
        public CardGameProjectListPage()
        {
			ViewModel = ViewModelFactory.NewList(new CardGameProjectSection());

            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
			NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
			ShellPage.Current.ShellControl.SelectItem("1467f99d-1413-4a85-bf0e-b396445d4bc9");
			ShellPage.Current.ShellControl.SetCommandBar(commandBar);
			if (e.NavigationMode == NavigationMode.New)
            {			
				await this.ViewModel.LoadDataAsync();
                this.ScrollToTop();
			}			
            base.OnNavigatedTo(e);
        }

    }
}
