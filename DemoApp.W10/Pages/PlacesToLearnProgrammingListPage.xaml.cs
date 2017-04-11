//---------------------------------------------------------------------------
//
// <copyright file="PlacesToLearnProgrammingListPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>4/11/2017 11:26:20 AM</createdOn>
//
//---------------------------------------------------------------------------

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using AppStudio.DataProviders.LocalStorage;
using DemoApp.Sections;
using DemoApp.ViewModels;
using AppStudio.Uwp;

namespace DemoApp.Pages
{
    public sealed partial class PlacesToLearnProgrammingListPage : Page
    {
	    public ListViewModel ViewModel { get; set; }
        public PlacesToLearnProgrammingListPage()
        {
			ViewModel = ViewModelFactory.NewList(new PlacesToLearnProgrammingSection());

            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
			NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
			ShellPage.Current.ShellControl.SelectItem("f51fc4f7-b7b7-4deb-9bde-f61cb2bdbc21");
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
