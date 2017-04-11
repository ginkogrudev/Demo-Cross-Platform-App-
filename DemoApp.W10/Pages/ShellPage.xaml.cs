using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media.Imaging;

using AppStudio.Uwp;
using AppStudio.Uwp.Controls;
using AppStudio.Uwp.Navigation;

using DemoApp.Navigation;

namespace DemoApp.Pages
{
    public sealed partial class ShellPage : Page
    {
        public static ShellPage Current { get; private set; }

        public ShellControl ShellControl
        {
            get { return shell; }
        }

        public Frame AppFrame
        {
            get { return frame; }
        }

        public ShellPage()
        {
            InitializeComponent();

            this.DataContext = this;
            ShellPage.Current = this;

            this.SizeChanged += OnSizeChanged;
            if (SystemNavigationManager.GetForCurrentView() != null)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += ((sender, e) =>
                {
                    if (SupportFullScreen && ShellControl.IsFullScreen)
                    {
                        e.Handled = true;
                        ShellControl.ExitFullScreen();
                    }
                    else if (NavigationService.CanGoBack())
                    {
                        NavigationService.GoBack();
                        e.Handled = true;
                    }
                });
				
                NavigationService.Navigated += ((sender, e) =>
                {
                    if (NavigationService.CanGoBack())
                    {
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                    }
                    else
                    {
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    }
                });
            }
        }

		public bool SupportFullScreen { get; set; }

		#region NavigationItems
        public ObservableCollection<NavigationItem> NavigationItems
        {
            get { return (ObservableCollection<NavigationItem>)GetValue(NavigationItemsProperty); }
            set { SetValue(NavigationItemsProperty, value); }
        }

        public static readonly DependencyProperty NavigationItemsProperty = DependencyProperty.Register("NavigationItems", typeof(ObservableCollection<NavigationItem>), typeof(ShellPage), new PropertyMetadata(new ObservableCollection<NavigationItem>()));
        #endregion

		protected override void OnNavigatedTo(NavigationEventArgs e)
        {
#if DEBUG
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size { Width = 320, Height = 500 });
#endif
            NavigationService.Initialize(typeof(ShellPage), AppFrame);
			NavigationService.NavigateToPage<HomePage>(e);

            InitializeNavigationItems();

            Bootstrap.Init();
        }		        
		
		#region Navigation
        private void InitializeNavigationItems()
        {
            NavigationItems.Add(AppNavigation.NodeFromAction(
				"Home",
                "Home",
                (ni) => NavigationService.NavigateToRoot(),
                AppNavigation.IconFromSymbol(Symbol.Home)));
            NavigationItems.Add(AppNavigation.NodeFromAction(
				"59be312e-3799-4285-9a7a-77b26f5d301a",
                "Greetings traveler",                
                AppNavigation.ActionFromPage("GreetingsTravelerListPage"),
				AppNavigation.IconFromGlyph("\ue113")));

            NavigationItems.Add(AppNavigation.NodeFromAction(
				"1467f99d-1413-4a85-bf0e-b396445d4bc9",
                "Card Game Project ",                
                AppNavigation.ActionFromPage("CardGameProjectListPage"),
				AppNavigation.IconFromGlyph("\ue12a")));


            var menuSocialMediaItems = new List<NavigationItem>();

            menuSocialMediaItems.Add(AppNavigation.NodeFromAction(
				"0fd96c25-68a5-4b4d-8517-4d9958c2f6f0",
                "Facebook",
				AppNavigation.ActionFromPage("FacebookListPage"),
                null, new Image() { Source = new BitmapImage(new Uri("ms-appx:///Assets/DataImages/facebook-256-1.png")) }));

            menuSocialMediaItems.Add(AppNavigation.NodeFromAction(
				"0fd96c25-68a5-4b4d-8517-4d9958c2f6f0",
                "Interesting Videos",
				AppNavigation.ActionFromPage("InterestingVideosListPage"),
                null, new Image() { Source = new BitmapImage(new Uri("ms-appx:///Assets/DataImages/logo-2.png")) }));

            NavigationItems.Add(AppNavigation.NodeFromSubitems(
				"0fd96c25-68a5-4b4d-8517-4d9958c2f6f0",
                "Social Media",                
                menuSocialMediaItems,
				null, new Image() { Source = new BitmapImage(new Uri("ms-appx:///Assets/DataImages/Social-Media-Icons.png")) }));            

            NavigationItems.Add(AppNavigation.NodeFromAction(
				"f51fc4f7-b7b7-4deb-9bde-f61cb2bdbc21",
                "Places to learn programming ",                
                AppNavigation.ActionFromPage("PlacesToLearnProgrammingListPage"),
				null, new Image() { Source = new BitmapImage(new Uri("ms-appx:///Assets/DataImages/bookshelf32x32.png")) }));

            NavigationItems.Add(AppNavigation.NodeFromAction(
				"f5a7b304-0cfb-400b-a988-1917f5096c41",
                "Learning ",                
                AppNavigation.ActionFromPage("LearningListPage"),
				AppNavigation.IconFromGlyph("\ue1d3")));

            NavigationItems.Add(NavigationItem.Separator);

            NavigationItems.Add(AppNavigation.NodeFromControl(
				"About",
                "NavigationPaneAbout".StringResource(),
                new AboutPage(),
                AppNavigation.IconFromImage(new Uri("ms-appx:///Assets/about.png"))));
        }        
        #endregion        

		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateDisplayMode(e.NewSize.Width);
        }

        private void UpdateDisplayMode(double? width = null)
        {
            if (width == null)
            {
                width = Window.Current.Bounds.Width;
            }
            this.ShellControl.DisplayMode = width > 640 ? SplitViewDisplayMode.CompactOverlay : SplitViewDisplayMode.Overlay;
            this.ShellControl.CommandBarVerticalAlignment = width > 640 ? VerticalAlignment.Top : VerticalAlignment.Bottom;
        }

        private async void OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.F11)
            {
                if (SupportFullScreen)
                {
                    await ShellControl.TryEnterFullScreenAsync();
                }
            }
            else if (e.Key == Windows.System.VirtualKey.Escape)
            {
                if (SupportFullScreen && ShellControl.IsFullScreen)
                {
                    ShellControl.ExitFullScreen();
                }
                else
                {
                    NavigationService.GoBack();
                }
            }
        }
    }
}
