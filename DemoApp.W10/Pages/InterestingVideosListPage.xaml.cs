//---------------------------------------------------------------------------
//
// <copyright file="InterestingVideosListPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>4/11/2017 11:26:20 AM</createdOn>
//
//---------------------------------------------------------------------------

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using AppStudio.DataProviders.YouTube;
using DemoApp.Sections;
using DemoApp.ViewModels;
using AppStudio.Uwp;

namespace DemoApp.Pages
{
    public sealed partial class InterestingVideosListPage : Page
    {
	    public ListViewModel ViewModel { get; set; }
        public InterestingVideosListPage()
        {
			ViewModel = ViewModelFactory.NewList(new InterestingVideosSection());

            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
			NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
			ShellPage.Current.ShellControl.SelectItem("0fd96c25-68a5-4b4d-8517-4d9958c2f6f0");
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
