//---------------------------------------------------------------------------
//
// <copyright file="LearningListPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>4/11/2017 11:26:20 AM</createdOn>
//
//---------------------------------------------------------------------------

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using AppStudio.DataProviders.DynamicStorage;
using DemoApp.Sections;
using DemoApp.ViewModels;
using AppStudio.Uwp;

namespace DemoApp.Pages
{
    public sealed partial class LearningListPage : Page
    {
	    public ListViewModel ViewModel { get; set; }
        public LearningListPage()
        {
			ViewModel = ViewModelFactory.NewList(new LearningSection());

            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
			NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
			ShellPage.Current.ShellControl.SelectItem("f5a7b304-0cfb-400b-a988-1917f5096c41");
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
